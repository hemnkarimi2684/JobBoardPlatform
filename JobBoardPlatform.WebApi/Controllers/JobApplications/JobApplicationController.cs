using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.JobApplications;

[Route("api/[controller]s")]
[ApiController]
//[Authorize]
public class JobApplicationController : ControllerBase
{
    private readonly IJobApplicationService _jobApplicationService;

    public JobApplicationController(IJobApplicationService jobApplicationService)
    {
        _jobApplicationService = jobApplicationService;
    }

    [HttpPost]
    [Authorize(Roles = "JobSeeker")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateJobApplicationAsync([FromBody] CreateJobApplicationRequestDto createJobApplication)
    {
        await _jobApplicationService.CreateJobApplicationAsync(createJobApplication);

        return Ok(Result.Success());
    }

    [HttpPatch("{jobApplicationId:guid}")]
    [Authorize(Roles = "Admin,Employer")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> UpdateJobApplicationStatusAsync([FromRoute] Guid jobApplicationId, [FromBody] string statusName)
    {
        await _jobApplicationService.UpdateJobApplicationStatusAsync(jobApplicationId, statusName);

        return NoContent();
    }

    [HttpGet("by-advertisement/{advertisementId:guid}")]
    [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetAdvertisementJobApplicationsAsync([FromRoute] Guid advertisementId, [FromQuery] PagingRequestDto pagingRequest)
    {
        var result = await _jobApplicationService.GetAdvertisementJobApplicationsAsync(advertisementId, pagingRequest);

        return Ok(Result<Pagination<JobApplicationInfoResponseDto>>.Success(result));
    }

    [HttpGet("{jobApplicationId:guid}")]
    [Authorize(Roles = "Employer,Admin")]
    public async Task<IActionResult> GetJobApplicationByIdAsync([FromRoute] Guid jobApplicationId)
    {
        var result = await _jobApplicationService.GetJobApplicationByIdAsync(jobApplicationId);

        return Ok(Result<JobApplicationInfoResponseDto>.Success(result));
    }
}
