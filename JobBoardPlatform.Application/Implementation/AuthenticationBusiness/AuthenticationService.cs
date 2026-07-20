using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.AspNetCore.Identity;

namespace JobBoardPlatform.Application.Implementation.AuthenticationBusiness;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly ICompanyService _companyService;

    private readonly IJwtService _jwtService;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    public AuthenticationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICompanyService companyService, IJwtService jwtService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _companyService = companyService;
        _jwtService = jwtService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    #region Login methods

    public async Task<TokenLoginResponseDto> LoginByEmailOrPhoneNumberAndPassword(LoginRequestDto loginCommand)
    {
        var user = await _userManager.FindByEmailAsync(loginCommand.EmailOrPhoneNumber) ??
                   await _unitOfWork.UserRepository.FindByPhoneNumberAsync(loginCommand.EmailOrPhoneNumber);

        if (user == null)
            throw new ValidationException("Email/phone number or password is incorrect.");

        var result = await _signInManager.PasswordSignInAsync(user, loginCommand.Password, false, true);

        if (result.IsLockedOut)
            throw new UnauthorizedException("User is locked out. Please try again 15 minutes later.");

        if (result.IsNotAllowed)
            throw new ForbiddenException("Login is not allowed for this account.");

        if (!result.Succeeded)
            throw new ValidationException("Email/phone number or password is incorrect.");

        if (user.IsApproved == false)
            throw new ForbiddenException("Dear user, your account has not yet been verified; please try again later.");

        return await _jwtService.GenerateTokenAsync(user);
    }

    #endregion

    #region Register methods

    public async Task<EmployerRegisterResponseDto> RegisterEmployerAsync(RegisterEmployerRequestDto registerCommand)
    {
        var isDupplicateEmailOrPhoneNumber = await _unitOfWork.UserRepository.IsDuplicateEmailOrPhoneNumberAsync(registerCommand.Email, registerCommand.PhoneNumber);

        if (isDupplicateEmailOrPhoneNumber)
            throw new ConflictException("A user with the provided email or phone number already exists.");

        var user = new User(registerCommand.Email, registerCommand.PhoneNumber, false);

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var createUserResult = await _userManager.CreateAsync(user, registerCommand.Password);

            if (!createUserResult.Succeeded)
                throw new ValidationException(string.Join(" ", createUserResult.Errors.Select(e => e.Description)));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, RoleConstants.EmployerRoleName);

            if (!addToRoleResult.Succeeded)
                throw new ValidationException(string.Join(" ", addToRoleResult.Errors.Select(e => e.Description)));

            var createdCompanyId = await _companyService.CreateCompanyAsync(registerCommand.ToCreateCompanyRequestDto(user.Id));

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return new EmployerRegisterResponseDto(user.Id, createdCompanyId);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync();

            throw;
        }
    }

    public async Task<TokenLoginResponseDto> RegisterJobSeekerAsync(RegisterJobSeekerRequestDto registerCommand)
    {
        var isDupplicate = await _unitOfWork.UserRepository.IsDuplicateEmailOrPhoneNumberAsync(registerCommand.Email, registerCommand.PhoneNumber);

        if (isDupplicate)
            throw new ConflictException("A user with the provided email or phone number already exists.");

        var user = new User(registerCommand.Email, registerCommand.PhoneNumber, true);

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var createUserResult = await _userManager.CreateAsync(user, registerCommand.Password);

            if (!createUserResult.Succeeded)
                throw new ValidationException(string.Join(" ", createUserResult.Errors.Select(e => e.Description)));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, RoleConstants.JobSeekerRoleName);

            if (!addToRoleResult.Succeeded)
                throw new ValidationException(string.Join(" ", addToRoleResult.Errors.Select(e => e.Description)));

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync();

            throw;
        }

        return await _jwtService.GenerateTokenAsync(user);
    }

    #endregion
}
