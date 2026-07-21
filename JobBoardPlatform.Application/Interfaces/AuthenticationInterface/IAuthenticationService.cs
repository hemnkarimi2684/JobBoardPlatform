using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

namespace JobBoardPlatform.Application.Interfaces.AuthenticationInterface;

public interface IAuthenticationService
{
    Task<EmployerRegisterResponseDto> RegisterEmployerAsync(
        RegisterEmployerRequestDto registerCommand,
        CancellationToken cancellationToken = default);

    Task<TokenLoginResponseDto> RegisterJobSeekerAsync(
        RegisterJobSeekerRequestDto registerCommand,
        CancellationToken cancellationToken = default);

    Task<TokenLoginResponseDto> LoginByEmailOrPhoneNumberAndPassword(
        LoginRequestDto loginCommand,
        CancellationToken cancellationToken = default);
}
