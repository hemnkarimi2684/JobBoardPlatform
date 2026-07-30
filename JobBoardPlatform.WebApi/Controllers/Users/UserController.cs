using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Users;

[Route("api/users")]
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
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateProfileAsync(
        [FromBody] CreateProfileRequestDto createProfile,
        CancellationToken cancellationToken)
    {
        await _userService.CreateProfileAsync(createProfile, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPut("{userId:guid}")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> UpdateProfileAsync(
        [FromRoute] Guid userId,
        [FromBody] UpdateProfileRequestDto updateProfile,
        CancellationToken cancellationToken)
    {
        await _userService.UpdateProfileAsync(userId, updateProfile, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("{userId:guid}/download-image")]
    public async Task<IActionResult> DownloadUserImageAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _userService.DownloadUserImageAsync(userId, cancellationToken);

        return File(result.Data, result.ContentType, result.FileName);
    }

    [HttpPatch("{userId:guid}/upload-image")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> UploadUserImageAsync(
        [FromRoute] Guid userId,
        [FromForm] UploadUserImageRequestDto imageRequestDto,
        CancellationToken cancellationToken)
    {
        await _userService.UploadUserImageAsync(userId, imageRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("genders")]
    [AllowAnonymous]
    public async Task<IActionResult> GetGenders()
    {
        var result = _userService.GetGenders();

        return Ok(Result<List<EnumResponseDto>>.Success(result));
    }
}
