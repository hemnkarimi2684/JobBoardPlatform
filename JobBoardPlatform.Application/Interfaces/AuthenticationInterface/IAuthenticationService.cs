using JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Command;
using JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Result;

namespace JobBoardPlatform.Application.Interfaces.AuthenticationInterface;

public interface IAuthenticationService
{
    Task<EmployerRegisterResult> RegisterEmployerAsync(RegisterCommand registerCommand);

    Task<UserRegisterResult> RegisterUserAsync(RegisterCommand registerCommand);

    Task<TokenLoginResult> LoginByEmailOrPhoneNumberAndPassword(LoginCommand loginCommand);
}
