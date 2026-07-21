using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Users;

[Route("api/Users")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    [Authorize(Roles = "JobSeeker")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateProfileAsync(
        [FromBody] CreateProfileRequestDto createProfile,
        CancellationToken cancellationToken)
    {
        await _userService.CreateProfileAsync(createProfile, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPut("{userId:guid}")]
    [Authorize(Roles = "JobSeeker")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> UpdateProfileAsync(
        [FromRoute] Guid userId,
        [FromBody] UpdateProfileRequestDto updateProfile,
        CancellationToken cancellationToken)
    {
        await _userService.UpdateProfileAsync(userId, updateProfile, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("{userId:guid}/info")]
    [Authorize(Roles = "Admin,Employer,JobSeeker")]
    public async Task<IActionResult> GetUserProfileInfoAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserProfileInfoAsync(userId, cancellationToken);

        return Ok(Result<UserProfileInfoResponseDto>.Success(result));
    }

    [HttpPatch("{employerId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApprovedEmployerAsync(
        [FromRoute] Guid employerId,
        CancellationToken cancellationToken)
    {
        await _userService.ApprovedEmployerAsync(employerId, cancellationToken);

        return Ok(Result.Success());
    }
}
