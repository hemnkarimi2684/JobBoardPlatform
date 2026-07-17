using JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Interfaces.AuthenticationInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Authentications
{
    [Route("api/[controller]s")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register-employer")]
        public async Task<IActionResult> RegisterEmployerAsync(RegisterEmployerRequestDto register)
        {
            var result = await _authenticationService.RegisterEmployerAsync(register);

            return Ok(Result<EmployerRegisterResponseDto>.Success(result));
        }

        [HttpPost("register-jobSeeker")]
        public async Task<IActionResult> RegisterJobSeekerAsync(RegisterJobSeekerRequestDto register)
        {
            var result = await _authenticationService.RegisterJobSeekerAsync(register);

            return Ok(Result<TokenLoginResponseDto>.Success(result));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginByEmailOrPhoneNumberAndPassword(LoginRequestDto login)
        {
            var result = await _authenticationService.LoginByEmailOrPhoneNumberAndPassword(login);

            return Ok(Result<TokenLoginResponseDto>.Success(result));
        }
    }
}
