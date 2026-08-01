using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.JobApplications;

[Route("api/jobApplications")]
[ApiController]
[Authorize]
public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;

    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpPost]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateJobApplicationAsync(
        [FromBody] CreateJobApplicationRequestDto createJobApplication,
        CancellationToken cancellationToken)
    {
        await _jobApplicationService.CreateJobApplicationAsync(createJobApplication, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{jobApplicationId:guid}")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> UpdateJobApplicationStatusAsync(
        [FromRoute] Guid jobApplicationId,
        [FromBody] JobApplicationStatus status,
        CancellationToken cancellationToken)
    {
        await _jobApplicationService.UpdateJobApplicationStatusAsync(jobApplicationId, status, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("by-advertisement/{advertisementId:guid}")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> GetAdvertisementJobApplicationsAsync(
        [FromRoute] Guid advertisementId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _jobApplicationService.GetAdvertisementJobApplicationsAsync(advertisementId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{jobApplicationId:guid}")]
    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> GetJobApplicationByIdAsync(
        [FromRoute] Guid jobApplicationId,
        CancellationToken cancellationToken)
    {
        var result = await _jobApplicationService.GetJobApplicationByIdAsync(jobApplicationId, cancellationToken);

        return Ok(Result<JobApplicationDetailResponseDto>.Success(result));
    }

    [HttpGet("by-user/{userId:guid}")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> GetJobApplicationsByUserIdAsync(
        [FromRoute] Guid userId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _jobApplicationService.GetJobApplicationsByUserIdAsync(userId, pagingRequest, cancellationToken);

        return Ok(Result<Pagination<JobApplicationDetailResponseDto>>.Success(result));
    }

    [HttpPatch("{jobApplicationId:guid}/cancel")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> CancelJobApplicationAsync(
        [FromRoute] Guid jobApplicationId,
        CancellationToken cancellationToken)
    {
        await _jobApplicationService.CancelJobApplicationAsync(jobApplicationId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("statuses")]
    [AllowAnonymous]
    public async Task<IActionResult> GetJobApplicationStatuses()
    {
        var result = _jobApplicationService.GetJobApplicationStatuses();

        return Ok(Result<List<EnumResponseDto>>.Success(result));
    }
}
