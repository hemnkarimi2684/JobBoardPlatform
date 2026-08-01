using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.JobCategories;

[Route("api/admin/jobCategories")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminJobCategoriesController : ControllerBase
{
    private readonly IJobCategoryService _jobCategoryService;

    public AdminJobCategoriesController(IJobCategoryService jobCategoryService)
    {
        _jobCategoryService = jobCategoryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobCategoryAsync(
        [FromBody] CreateJobCategoryRequestDto jobCategoryRequestDto,
         CancellationToken cancellationToken)
    {
        await _jobCategoryService.CreateJobCategoryAsync(jobCategoryRequestDto, cancellationToken);

        return Ok(Result.Success());
    }
}
