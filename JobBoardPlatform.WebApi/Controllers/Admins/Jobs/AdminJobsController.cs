using JobBoardPlatform.Application.Common.Dto.RequestDto.JobDto;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Jobs;

[Route("api/admin/jobs")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminJobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public AdminJobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobAsync(
       [FromBody] CreateJobRequestDto createJobRequestDto,
        CancellationToken cancellationToken)
    {
        await _jobService.CreateJobAsync(createJobRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpDelete("{jobId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid jobId,
        CancellationToken cancellationToken)
    {
        await _jobService.SoftDeleteAsync(jobId, cancellationToken);

        return Ok(Result.Success());
    }
}
