using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Users;

[Route("api/admin/users")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminUsersController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminUsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId:guid}/profile")]
    public async Task<IActionResult> GetUserProfileByUserIdAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserProfileByUserIdAsync(userId, cancellationToken);

        return Ok(Result<UserProfileResponseDto>.Success(result));
    }

    [HttpGet("approved-employers")]
    public async Task<IActionResult> GetApprovedEmployersAsync(
        [FromQuery] PagingRequestDto pagingRequestDto)
    {
        var result = await _userService.GetApprovedEmployersAsync(pagingRequestDto);

        return Ok(result);
    }

    [HttpGet("unapproved-employers")]
    public async Task<IActionResult> GetUnapprovedEmployersAsync(
        [FromQuery] PagingRequestDto pagingRequestDto)
    {
        var result = await _userService.GetUnapprovedEmployersAsync(pagingRequestDto);

        return Ok(result);
    }

    [HttpGet("jobSeekers")]
    public async Task<IActionResult> GetJobSeekersAsync(
        [FromQuery] PagingRequestDto pagingRequestDto)
    {
        var result = await _userService.GetJobSeekersAsync(pagingRequestDto);

        return Ok(result);
    }

    [HttpGet("employers/{userId:guid}/company")]
    public async Task<IActionResult> GetEmployerWithCompanyAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetEmployerWithCompanyAsync(userId, cancellationToken);

        return Ok(Result<EmployerWithCompanyResponseDto>.Success(result));
    }

    [HttpPatch("{userId:guid}/approved-employer")]
    public async Task<IActionResult> ApprovedEmployerAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        await _userService.ApprovedEmployerAsync(userId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{userId:guid}/reject-employer")]
    public async Task<IActionResult> RejectEmployerAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        await _userService.RejectEmployerAsync(userId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{userId:guid}/activate-jobSeeker")]
    public async Task<IActionResult> ActivateJobSeekerAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        await _userService.ActivateJobSeekerAsync(userId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{userId:guid}/deactivate-jobSeeker")]
    public async Task<IActionResult> DeactivateJobSeekerAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        await _userService.DeactivateJobSeekerAsync(userId, cancellationToken);

        return Ok(Result.Success());
    }
}
