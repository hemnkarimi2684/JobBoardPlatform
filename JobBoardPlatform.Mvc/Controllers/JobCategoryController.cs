using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Mvc.Models.JobCategory;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.Mvc.Controllers;

public class JobCategoryController : Controller
{
    private readonly IJobCategoryService _jobCategoryService;

    public JobCategoryController(IJobCategoryService jobCategoryService)
    {
        _jobCategoryService = jobCategoryService;
    }

    public async Task<IActionResult> Index(string text, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto { Text = text },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        ViewBag.Text = text;

        return View(JobCategoryIndexViewModel.FromResponseDto(result));
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _jobCategoryService.GetJobCategoryByIdAsync(id, cancellationToken);

        return View(JobCategoryDetailsViewModel.FromResponseDto(category));
    }
}
