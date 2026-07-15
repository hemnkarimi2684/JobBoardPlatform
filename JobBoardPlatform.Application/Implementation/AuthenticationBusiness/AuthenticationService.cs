using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Command;
using JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Core.Entities.Common.Data;

namespace JobBoardPlatform.Application.Implementation.AuthenticationBusiness;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly ICompanyService _companyService;

    private readonly IJwtService _jwtService;

    public AuthenticationService(IUnitOfWork unitOfWork, ICurrentUser currentUser, ICompanyService companyService, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _companyService = companyService;
        _jwtService = jwtService;
    }

    public async Task<TokenLoginResult> LoginByEmailOrPhoneNumberAndPassword(LoginCommand loginCommand)
    {
        var user = await _unitOfWork.UserManager.FindByEmailAsync(loginCommand.EmailOrPhoneNumber) ??
                   await _unitOfWork.UserRepository.FindByPhoneNumberAsync(loginCommand.EmailOrPhoneNumber);

        if (user == null)
            throw new NotFoundException("No user was found with the provided information.");

        var isPasswordValid = await _unitOfWork.UserManager.CheckPasswordAsync(user, loginCommand.Password);

        if (isPasswordValid)
            throw new ValidationException("The login password is invalid.");

        return await _jwtService.GenerateTokenAsync(user);
    }

    public Task<EmployerRegisterResult> RegisterEmployerAsync(RegisterCommand registerCommand)
    {
        throw new NotImplementedException();
    }

    public Task<UserRegisterResult> RegisterUserAsync(RegisterCommand registerCommand)
    {
        throw new NotImplementedException();
    }
}
