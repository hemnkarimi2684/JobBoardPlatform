using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.JobCategories;

[Route("api/[controller]")]
[ApiController]
public class JobCategoryController : ControllerBase
{
    private readonly IJobCategoryService _jobCategoryService;

    public JobCategoryController(IJobCategoryService jobCategoryService)
    {
        _jobCategoryService = jobCategoryService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateJobCategoryAsync(
        CreateJobCategoryRequestDto jobCategoryRequestDto,
        CancellationToken cancellationToken)
    {
        await _jobCategoryService.CreateJobCategoryAsync(jobCategoryRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllJobCategoriesAsync(
        string text,
        PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _jobCategoryService.GetAllJobCategoriesAsync(text, pagingRequestDto, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{jobCategoryId:guid}")]
    public async Task<IActionResult> GetJobCategoryByIdAsync(
        Guid jobCategoryId,
        CancellationToken cancellationToken)
    {
        var result = await _jobCategoryService.GetJobCategoryByIdAsync(jobCategoryId, cancellationToken);

        return Ok(Result<JobCategoryDetailResponseDto>.Success(result));
    }
}
