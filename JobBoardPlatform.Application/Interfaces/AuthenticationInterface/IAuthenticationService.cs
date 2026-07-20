using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

namespace JobBoardPlatform.Application.Interfaces.AuthenticationInterface;

public interface IAuthenticationService
{
    Task<EmployerRegisterResponseDto> RegisterEmployerAsync(RegisterEmployerRequestDto registerCommand);

    Task<TokenLoginResponseDto> RegisterJobSeekerAsync(RegisterJobSeekerRequestDto registerCommand);

    Task<TokenLoginResponseDto> LoginByEmailOrPhoneNumberAndPassword(LoginRequestDto loginCommand);
}
