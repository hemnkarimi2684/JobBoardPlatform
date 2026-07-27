using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobDto;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Jobs;

[Route("api/jobs")]
[ApiController]
[Authorize]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateJobAsync(
       [FromBody] CreateJobRequestDto createJobRequestDto,
        CancellationToken cancellationToken)
    {
        await _jobService.CreateJobAsync(createJobRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllJobsAsync(
      [FromQuery] TextRequestDto textRequestDto,
      [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _jobService.GetAllJobsAsync(textRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{jobId:guid}/advertisements")]
    [AllowAnonymous]
    public async Task<IActionResult> GetJobAdvertisementsAsync(
       [FromRoute] Guid jobId,
       [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _jobService.GetJobAdvertisementsAsync(jobId, pagingRequestDto, cancellationToken);

        return Ok(result);
    }
}
