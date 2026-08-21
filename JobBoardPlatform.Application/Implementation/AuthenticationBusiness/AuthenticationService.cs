using JobBoardPlatform.Application.Common.AccessClaims.UserClaim;
using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Application.Interfaces.RefreshTokenInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JobBoardPlatform.Application.Implementation.AuthenticationBusiness;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICompanyService _companyService;

    private readonly IJwtService _jwtService;

    private readonly IRefreshTokenService _refreshTokenService;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    public AuthenticationService(
        IUnitOfWork unitOfWork,
        ICompanyService companyService,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService, 
        UserManager<User> userManager, 
        SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _companyService = companyService;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    #region Login methods

    public async Task<TokenLoginResponseDto> LoginByEmailOrPhoneNumberAndPassword(
        LoginRequestDto loginCommand,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(loginCommand.EmailOrPhoneNumber) ??
                   await _unitOfWork.UserRepository.FindByPhoneNumberAsync(loginCommand.EmailOrPhoneNumber, cancellationToken);

        if (user == null)
            throw new ValidationException("Email/phone number or password is incorrect.");

        if (user.IsApproved == false)
            throw new ForbiddenException("“Your account is pending administrator approval.”");

        if (user.IsActive == false)
            throw new ForbiddenException("Your account is deactivated. Please contact support.");

        var result = await _signInManager.PasswordSignInAsync(user, loginCommand.Password, false, true);

        if (result.IsLockedOut)
            throw new UnauthorizedException("User is locked out. Please try again 15 minutes later.");

        if (result.IsNotAllowed)
            throw new ForbiddenException("Login is not allowed for this account.");

        if (!result.Succeeded)
            throw new ValidationException("Email/phone number or password is incorrect.");

        return await _jwtService.GenerateTokenAsync(user);
    }

    public async Task LogoutAsync(
        LogoutRequestDto logoutRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await _refreshTokenService.RevokeAsync(logoutRequest.RefreshToken, cancellationToken);

        if (!result)
            throw new ValidationException("Invalid refresh token or session has already been closed.");
    }

    public async Task<TokenLoginResponseDto> RefreshAsync(
        RefreshRequestDto refreshRequest,
        CancellationToken cancellationToken = default)
    {
        //دریافت توکن از دیتابیس 
        var refreshToken = await _refreshTokenService.GetRefreshTokenByTokenAsync(refreshRequest.RefreshToken, cancellationToken, true);

        // بررسی فعال بودن توکن
        if (!refreshToken.IsActive)
        {
            // تشخیص اینکه ایا استفاده مجدد داره میشه و همینطور برای تشخیص اتک 
            if (refreshToken.IsRevoked && refreshToken.RevokedAt is not null)
                await _refreshTokenService.RevokeAllActiveTokensAsync(refreshToken.UserId, cancellationToken);

            throw new UnauthorizedException("Session has expired or is invalid. Please log in again.");
        }

        //پیدا کردن کاربری که دارای این توکن است 
        var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());

        if (user == null)
            throw new NotFoundException("User associated with this token was not found.");

        if (user.IsApproved == false)
            throw new ForbiddenException("Your account is pending administrator approval");

        if (user.IsActive == false)
            throw new ForbiddenException("Your account is deactivated. Please contact support");

        // منقضی کردن توکن فعلی که داره استفاده میشه برای اینکه کلا یک بار مصرف باشه 
        refreshToken.Revoke();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //حالا تولید یه رفرش توکن و اکسس توکن جدید
        return await _jwtService.GenerateTokenAsync(user, cancellationToken);
    }

    #endregion

    #region Register methods

    public async Task<EmployerRegisterResponseDto> RegisterEmployerAsync(
        RegisterEmployerRequestDto registerCommand,
        CancellationToken cancellationToken = default)
    {
        var isDupplicateEmailOrPhoneNumber = await _unitOfWork.UserRepository.IsDuplicateEmailOrPhoneNumberAsync(
            registerCommand.Email,
            registerCommand.PhoneNumber,
            cancellationToken);

        if (isDupplicateEmailOrPhoneNumber)
            throw new ConflictException("A user with the provided email or phone number already exists.");

        var user = new User(registerCommand.Email, registerCommand.PhoneNumber, false);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var createUserResult = await _userManager.CreateAsync(user, registerCommand.Password);

            if (!createUserResult.Succeeded)
                throw new ValidationException(string.Join(" ", createUserResult.Errors.Select(e => e.Description)));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, RoleConstants.EmployerRoleName);

            if (!addToRoleResult.Succeeded)
                throw new ValidationException(string.Join(" ", addToRoleResult.Errors.Select(e => e.Description)));

            var createdCompanyId = await _companyService.CreateCompanyAsync(registerCommand.ToCreateCompanyRequestDto(user.Id));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return new EmployerRegisterResponseDto
            {
                EmployerId = user.Id,
                CompanyId = createdCompanyId
            };
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }
    }

    public async Task<TokenLoginResponseDto> RegisterJobSeekerAsync(
        RegisterJobSeekerRequestDto registerCommand,
        CancellationToken cancellationToken = default)
    {
        var isDupplicate = await _unitOfWork.UserRepository.IsDuplicateEmailOrPhoneNumberAsync(
            registerCommand.Email,
            registerCommand.PhoneNumber,
            cancellationToken);

        if (isDupplicate)
            throw new ConflictException("A user with the provided email or phone number already exists.");

        var user = new User(registerCommand.Email, registerCommand.PhoneNumber, true);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var createUserResult = await _userManager.CreateAsync(user, registerCommand.Password);

            if (!createUserResult.Succeeded)
                throw new ValidationException(string.Join(" ", createUserResult.Errors.Select(e => e.Description)));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, RoleConstants.JobSeekerRoleName);

            if (!addToRoleResult.Succeeded)
                throw new ValidationException(string.Join(" ", addToRoleResult.Errors.Select(e => e.Description)));

            var claim = new Claim(UserClaims.JobSeekerClaimType, UserClaims.IsActiveClaimValue);

            var jobSeekerClaims = await _userManager.GetClaimsAsync(user);

            if (!jobSeekerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                var result = await _userManager.AddClaimAsync(user, claim);

                if (!result.Succeeded)
                    throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }

        return await _jwtService.GenerateTokenAsync(user);
    }

    #endregion
}
