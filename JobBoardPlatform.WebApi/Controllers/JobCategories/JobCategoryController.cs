using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.JobCategories;

[Route("api/jobCategories")]
[ApiController]
[Authorize]
public class JobCategoryController : ControllerBase
{
    private readonly IJobCategoryService _jobCategoryService;

    public JobCategoryController(IJobCategoryService jobCategoryService)
    {
        _jobCategoryService = jobCategoryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllJobCategoriesAsync(
       [FromQuery] TextRequestDto textRequestDto,
       [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _jobCategoryService.GetAllJobCategoriesAsync(textRequestDto, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{jobCategoryId:guid}/detail")]
    [AllowAnonymous]
    public async Task<IActionResult> GetJobCategoryByIdAsync(
       [FromRoute] Guid jobCategoryId,
        CancellationToken cancellationToken)
    {
        var result = await _jobCategoryService.GetJobCategoryByIdAsync(jobCategoryId, cancellationToken);

        return Ok(Result<JobCategoryDetailResponseDto>.Success(result));
    }
}
