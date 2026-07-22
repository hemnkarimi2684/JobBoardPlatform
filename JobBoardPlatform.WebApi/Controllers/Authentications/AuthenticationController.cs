using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.Application.Interfaces.RefreshTokenInterface;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Authentications;

[Route("api/[controller]s")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register-employer")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> RegisterEmployerAsync(
      [FromBody] RegisterEmployerRequestDto register,
        CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterEmployerAsync(register, cancellationToken);

        return Ok(Result<EmployerRegisterResponseDto>.Success(result));
    }

    [HttpPost("register-jobSeeker")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> RegisterJobSeekerAsync(
       [FromBody] RegisterJobSeekerRequestDto register,
        CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RegisterJobSeekerAsync(register, cancellationToken);

        return Ok(Result<TokenLoginResponseDto>.Success(result));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginByEmailOrPhoneNumberAndPassword(
       [FromBody] LoginRequestDto login,
        CancellationToken cancellationToken)
    {
        var result = await _authenticationService.LoginByEmailOrPhoneNumberAndPassword(login);

        return Ok(Result<TokenLoginResponseDto>.Success(result));
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync(
       [FromBody] RefreshRequestDto refreshRequest,
        CancellationToken cancellationToken)
    {
        var result = await _authenticationService.RefreshAsync(refreshRequest, cancellationToken);

        return Ok(Result<TokenLoginResponseDto>.Success(result));
    }

    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(
       [FromBody] LogoutRequestDto logoutRequest,
        CancellationToken cancellationToken)
    {
        await _authenticationService.LogoutAsync(logoutRequest, cancellationToken);

        return Ok(Result.Success());
    }
}
