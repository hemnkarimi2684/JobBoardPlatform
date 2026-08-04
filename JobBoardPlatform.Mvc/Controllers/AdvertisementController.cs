using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

public class AdvertisementController : Controller
{
    private readonly IAdvertisementService _advertisementService;
    private readonly IJobService _jobService;
    private readonly ICityService _cityService;
    private readonly ISkillService _skillService;
    private readonly IJobCategoryService _jobCategoryService;
    private readonly IUserService _userService;

    public AdvertisementController(
        IAdvertisementService advertisementService,
        IJobService jobService,
        ICityService cityService,
        ISkillService skillService,
        IJobCategoryService jobCategoryService,
        IUserService userService)
    {
        _advertisementService = advertisementService;
        _jobService = jobService;
        _cityService = cityService;
        _skillService = skillService;
        _jobCategoryService = jobCategoryService;
        _userService = userService;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _advertisementService.GetActiveAdvertisementsAsync(
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string searchTerm, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _advertisementService.SearchAdvertisementsAsync(
            new AdvertisementSearchRequestDto { SearchTerm = searchTerm },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        ViewBag.SearchTerm = searchTerm;

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> Filter(Guid? jobCategoryId, decimal? minimumSalary, decimal? maximumSalary, CollaborationType? collaborationType, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _advertisementService.FilterAdvertisementsAsync(
            new AdvertisementFilterRequestDto
            {
                JobCategoryId = jobCategoryId,
                MinimumSalary = minimumSalary,
                MaximumSalary = maximumSalary,
                CollaborationType = collaborationType
            },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        var categories = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.JobCategories = new SelectList(categories.Data, nameof(JobCategoryResponseDto.JobCategoryId), nameof(JobCategoryResponseDto.Name));
        ViewBag.CollaborationTypes = Enum.GetValues<CollaborationType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();

        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var advertisement = await _advertisementService.GetAdvertisementInfoByIdAsync(id, cancellationToken);

            return View(advertisement);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> MyAds(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var employer = await _userService.GetEmployerWithCompanyAsync(CurrentUserId(), cancellationToken);

        var result = await _advertisementService.GetAdvertisementsByCompanyAsync(
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            employer.CompanyId,
            cancellationToken);

        return View(result);
    }

    [Authorize(Roles = "Employer")]
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        await PopulateSelectListsAsync(cancellationToken);

        return View();
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateAdvertisementRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }

        try
        {
            await _advertisementService.CreateAdvertisementAsync(model, cancellationToken);

            TempData["Success"] = "آگهی با موفقیت ثبت شد.";

            return RedirectToAction(nameof(MyAds));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var advertisement = await _advertisementService.GetAdvertisementInfoByIdAsync(id, cancellationToken);

            await PopulateSelectListsAsync(cancellationToken);

            return View(new UpdateAdvertisementRequestDto
            {
                Description = advertisement.Description,
                MinimumAge = advertisement.MinimumAge,
                MaximumAge = advertisement.MaximumAge,
                MinimumSalary = advertisement.MinimumSalary,
                MaximumSalary = advertisement.MaximumSalary,
                ExperienceLevel = advertisement.ExperienceLevel,
                CollaborationType = advertisement.CollaborationType
            });
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, UpdateAdvertisementRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }

        try
        {
            await _advertisementService.UpdateAdvertisementAsync(id, model, cancellationToken);

            TempData["Success"] = "آگهی با موفقیت ویرایش شد.";

            return RedirectToAction(nameof(MyAds));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

    private async Task PopulateSelectListsAsync(CancellationToken cancellationToken)
    {
        var jobs = await _jobService.GetAllJobsAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var cities = await _cityService.GetAllCitiesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var skills = await _skillService.GetAllSkillsAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var employer = await _userService.GetEmployerWithCompanyAsync(CurrentUserId(), cancellationToken);

        ViewBag.Jobs = new SelectList(jobs.Data, "JobId", "Name");
        ViewBag.Cities = new SelectList(cities.Data, nameof(CityDetailResponseDto.CityId), nameof(CityDetailResponseDto.CityName));
        ViewBag.Skills = new MultiSelectList(skills.Data, nameof(SkillDetailResponseDto.SkillId), nameof(SkillDetailResponseDto.SkillName));
        ViewBag.CollaborationTypes = Enum.GetValues<CollaborationType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
        ViewBag.DefaultCompanyId = employer.CompanyId;
    }
}
