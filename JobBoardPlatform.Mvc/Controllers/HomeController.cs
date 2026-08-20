using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Mvc.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobBoardPlatform.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly IAdvertisementService _advertisementService;
    private readonly IJobCategoryService _jobCategoryService;

    public HomeController(IAdvertisementService advertisementService, IJobCategoryService jobCategoryService)
    {
        _advertisementService = advertisementService;
        _jobCategoryService = jobCategoryService;
    }

    public async Task<IActionResult> Index(
        string? searchTerm,
        Guid? jobCategoryId,
        CollaborationType? collaborationType,
        decimal? minimumSalary,
        decimal? maximumSalary,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        var hasCriteria = !string.IsNullOrWhiteSpace(searchTerm)
            || jobCategoryId.HasValue
            || collaborationType.HasValue
            || minimumSalary.HasValue
            || maximumSalary.HasValue;

        var paging = new PagingRequestDto { PageNumber = pageNumber, PageSize = 6 };

        Pagination<AdvertisementDetailResponseDto> result;

        if (hasCriteria)
        {
            result = await _advertisementService.SearchAndFilterAdvertisementsAsync(
                new AdvertisementSearchRequestDto { SearchTerm = searchTerm },
                new AdvertisementFilterRequestDto
                {
                    JobCategoryId = jobCategoryId,
                    CollaborationType = collaborationType,
                    MinimumSalary = minimumSalary,
                    MaximumSalary = maximumSalary
                },
                paging,
                cancellationToken);
        }
        else
        {
            result = await _advertisementService.GetActiveAdvertisementsAsync(paging, cancellationToken);
        }

        var categories = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.JobCategories = new SelectList(
            categories.Data,
            nameof(JobCategoryResponseDto.JobCategoryId),
            nameof(JobCategoryResponseDto.Name));

        ViewBag.CollaborationTypes = Enum.GetValues<CollaborationType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();

        var model = HomeViewModel.FromResponseDto(
            result,
            searchTerm,
            jobCategoryId,
            collaborationType,
            minimumSalary,
            maximumSalary);

        return View(model);
    }

    public IActionResult Error()
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;

        return View();
    }

    public IActionResult NotFoundPage()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;

        return View("NotFound");
    }

    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;

        return View();
    }
}
