using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

namespace JobBoardPlatform.Application.Interfaces.AuthenticationInterface;

public interface IAuthenticationService
{
    Task<EmployerRegisterResponseDto> RegisterEmployerAsync(RegisterRequestDto registerCommand);

    Task<TokenLoginResponseDto> RegisterJobSeekerAsync(RegisterRequestDto registerCommand);

    Task<TokenLoginResponseDto> LoginByEmailOrPhoneNumberAndPassword(LoginRequestDto loginCommand);
}
