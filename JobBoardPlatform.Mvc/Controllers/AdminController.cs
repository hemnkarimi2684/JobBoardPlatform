using JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EmailTemplateDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ProvinceDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ProvinceDto;
using JobBoardPlatform.Application.Interfaces.AdminDashboardInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.Application.Interfaces.ProvinceInterface;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminDashboardService _adminDashboardService;
    private readonly IUserService _userService;
    private readonly IAdvertisementService _advertisementService;
    private readonly ICityService _cityService;
    private readonly IProvinceService _provinceService;
    private readonly IJobCategoryService _jobCategoryService;
    private readonly ISkillService _skillService;
    private readonly IJobService _jobService;
    private readonly IEmailService _emailService;

    public AdminController(
        IAdminDashboardService adminDashboardService,
        IUserService userService,
        IAdvertisementService advertisementService,
        ICityService cityService,
        IProvinceService provinceService,
        IJobCategoryService jobCategoryService,
        ISkillService skillService,
        IJobService jobService,
        IEmailService emailService)
    {
        _adminDashboardService = adminDashboardService;
        _userService = userService;
        _advertisementService = advertisementService;
        _cityService = cityService;
        _provinceService = provinceService;
        _jobCategoryService = jobCategoryService;
        _skillService = skillService;
        _jobService = jobService;
        _emailService = emailService;
    }

    public async Task<IActionResult> Dashboard()
    {
        var counts = await _adminDashboardService.GetCountsAsync();

        return View(counts);
    }

    public async Task<IActionResult> Employers(CancellationToken cancellationToken = default)
    {
        var approved = await _userService.GetApprovedEmployersAsync(new PagingRequestDto { PageNumber = 1, PageSize = 100 });
        var unapproved = await _userService.GetUnapprovedEmployersAsync(new PagingRequestDto { PageNumber = 1, PageSize = 100 });

        ViewBag.ApprovedEmployers = approved.Data;
        ViewBag.UnapprovedEmployers = unapproved.Data;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApproveEmployer(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.ApprovedEmployerAsync(userId, cancellationToken);

        TempData["Success"] = "Employer was approved successfully.";

        return RedirectToAction(nameof(Employers));
    }

    [HttpPost]
    public async Task<IActionResult> RejectEmployer(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.RejectEmployerAsync(userId, cancellationToken);

        TempData["Success"] = "Employer was rejected successfully.";

        return RedirectToAction(nameof(Employers));
    }

    public async Task<IActionResult> JobSeekers(CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetJobSeekersAsync(new PagingRequestDto { PageNumber = 1, PageSize = 100 });

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> ActivateJobSeeker(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.ActivateJobSeekerAsync(userId, cancellationToken);

        TempData["Success"] = "Job seeker was activated successfully.";

        return RedirectToAction(nameof(JobSeekers));
    }

    [HttpPost]
    public async Task<IActionResult> DeactivateJobSeeker(Guid userId, CancellationToken cancellationToken)
    {
        await _userService.DeactivateJobSeekerAsync(userId, cancellationToken);

        TempData["Success"] = "Job seeker was deactivated successfully.";

        return RedirectToAction(nameof(JobSeekers));
    }

    public async Task<IActionResult> Advertisements(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _advertisementService.GetAllAdvertisementsAsync(
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> ActivateAdvertisement(Guid advertisementId, CancellationToken cancellationToken)
    {
        await _advertisementService.ActivateAdvertisementAsync(advertisementId, cancellationToken);

        TempData["Success"] = "Advertisement was activated successfully.";

        return RedirectToAction(nameof(Advertisements));
    }

    [HttpPost]
    public async Task<IActionResult> DeactivateAdvertisement(Guid advertisementId, CancellationToken cancellationToken)
    {
        await _advertisementService.DeactivateAdvertisementAsync(advertisementId, cancellationToken);

        TempData["Success"] = "Advertisement was deactivated successfully.";

        return RedirectToAction(nameof(Advertisements));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAdvertisement(Guid advertisementId, CancellationToken cancellationToken)
    {
        await _advertisementService.SoftDeleteAdvertisementAsync(advertisementId, cancellationToken);

        TempData["Success"] = "Advertisement was deleted successfully.";

        return RedirectToAction(nameof(Advertisements));
    }

    [HttpPost]
    public async Task<IActionResult> PromoteAdvertisement(Guid advertisementId, int durationInDays, CancellationToken cancellationToken)
    {
        await _advertisementService.PromoteAdvertisementAsync(advertisementId, durationInDays, cancellationToken);

        TempData["Success"] = "Advertisement was promoted successfully.";

        return RedirectToAction(nameof(Advertisements));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DemoteAdvertisement(
    Guid advertisementId,
    CancellationToken cancellationToken)
    {
        await _advertisementService.DemoteAdvertisementAsync(
            advertisementId,
            cancellationToken);

        TempData["Success"] = "Advertisement was removed from featured successfully.";

        return RedirectToAction(nameof(Advertisements));
    }

    public async Task<IActionResult> Cities(CancellationToken cancellationToken = default)
    {
        var cities = await _cityService.GetAllCitiesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var provinces = await _provinceService.GetAllProvincesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.Cities = cities.Data;
        ViewBag.Provinces = new SelectList(provinces.Data, nameof(ProvinceResponseDto.ProvinceId), nameof(ProvinceResponseDto.Name));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCity(CreateCityRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The city data is invalid.";
            return RedirectToAction(nameof(Cities));
        }

        await _cityService.CreateCityAsync(model, cancellationToken);

        TempData["Success"] = "City was created successfully.";

        return RedirectToAction(nameof(Cities));
    }

    public async Task<IActionResult> Provinces(CancellationToken cancellationToken = default)
    {
        var result = await _provinceService.GetAllProvincesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProvince(CreateProvinceRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The province data is invalid.";
            return RedirectToAction(nameof(Provinces));
        }

        await _provinceService.CreateProvinceAsync(model, cancellationToken);

        TempData["Success"] = "Province was created successfully.";

        return RedirectToAction(nameof(Provinces));
    }

    public async Task<IActionResult> JobCategories(CancellationToken cancellationToken = default)
    {
        var result = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobCategory(CreateJobCategoryRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The job category data is invalid.";
            return RedirectToAction(nameof(JobCategories));
        }

        await _jobCategoryService.CreateJobCategoryAsync(model, cancellationToken);

        TempData["Success"] = "Job category was created successfully.";

        return RedirectToAction(nameof(JobCategories));
    }

    public async Task<IActionResult> Skills(CancellationToken cancellationToken = default)
    {
        var result = await _skillService.GetAllSkillsAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSkill(CreateSkillRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The skill data is invalid.";
            return RedirectToAction(nameof(Skills));
        }

        await _skillService.CreateSkillAsync(model, cancellationToken);

        TempData["Success"] = "Skill was created successfully.";

        return RedirectToAction(nameof(Skills));
    }

    public async Task<IActionResult> Jobs(CancellationToken cancellationToken = default)
    {
        var jobs = await _jobService.GetAllJobsAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var categories = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.Jobs = jobs.Data;
        ViewBag.JobCategories = new SelectList(categories.Data, nameof(JobCategoryResponseDto.JobCategoryId), nameof(JobCategoryResponseDto.Name));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob(CreateJobRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The job data is invalid.";
            return RedirectToAction(nameof(Jobs));
        }

        await _jobService.CreateJobAsync(model, cancellationToken);

        TempData["Success"] = "Job was created successfully.";

        return RedirectToAction(nameof(Jobs));
    }

    public async Task<IActionResult> EmailTemplates(CancellationToken cancellationToken = default)
    {
        var result = await _emailService.GetAllAsync(
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateTemplate(Guid id, UpdateTemplateRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The email template data is invalid.";
            return RedirectToAction(nameof(EmailTemplates));
        }

        await _emailService.UpdateTemplateAsync(id, model, cancellationToken);

        TempData["Success"] = "Email template was updated successfully.";

        return RedirectToAction(nameof(EmailTemplates));
    }

    [HttpPost]
    public async Task<IActionResult> ActivateTemplate(Guid id, CancellationToken cancellationToken)
    {
        await _emailService.ActivateTemplateAsync(id, cancellationToken);

        TempData["Success"] = "Email template was activated successfully.";

        return RedirectToAction(nameof(EmailTemplates));
    }

    [HttpPost]
    public async Task<IActionResult> DeactivateTemplate(Guid id, CancellationToken cancellationToken)
    {
        await _emailService.DeactivateTemplateAsync(id, cancellationToken);

        TempData["Success"] = "Email template was deactivated successfully.";

        return RedirectToAction(nameof(EmailTemplates));
    }
}
