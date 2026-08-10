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
using JobBoardPlatform.Mvc.Models.Admin;
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

        return View(DashboardViewModel.FromResponseDto(counts));
    }

    [HttpGet]
    public async Task<IActionResult> Employers(
        int approvedPage = 1,
        int unapprovedPage = 1)
    {
        const int pageSize = 10;

        approvedPage = approvedPage <= 0 ? 1 : approvedPage;
        unapprovedPage = unapprovedPage <= 0 ? 1 : unapprovedPage;

        var approvedResponse =
            await _userService.GetApprovedEmployersAsync(
                new PagingRequestDto
                {
                    PageNumber = approvedPage,
                    PageSize = pageSize
                });

        var unapprovedResponse =
            await _userService.GetUnapprovedEmployersAsync(
                new PagingRequestDto
                {
                    PageNumber = unapprovedPage,
                    PageSize = pageSize
                });

        var model = EmployersViewModel.FromResponseDto(
            approvedResponse,
            unapprovedResponse);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveEmployer(
        Guid userId,
        int approvedPage = 1,
        int unapprovedPage = 1,
        CancellationToken cancellationToken = default)
    {
        await _userService.ApprovedEmployerAsync(userId, cancellationToken);

        TempData["Success"] = "Employer was approved successfully.";

        return RedirectToAction(nameof(Employers), new
        {
            approvedPage,
            unapprovedPage
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectEmployer(
        Guid userId,
        int approvedPage = 1,
        int unapprovedPage = 1,
        CancellationToken cancellationToken = default)
    {
        await _userService.RejectEmployerAsync(userId, cancellationToken);

        TempData["Success"] = "Employer was rejected successfully.";

        return RedirectToAction(nameof(Employers), new
        {
            approvedPage,
            unapprovedPage
        });
    }

    [HttpGet]
    public async Task<IActionResult> JobSeekers(
        int pageNumber = 1)
    {
        const int pageSize = 10;

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var result = await _userService.GetJobSeekersAsync(
            new PagingRequestDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

        return View(JobSeekersViewModel.FromResponseDto(result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateJobSeeker(
        Guid userId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _userService.ActivateJobSeekerAsync(userId, cancellationToken);

        TempData["Success"] = "Job seeker was activated successfully.";

        return RedirectToAction(nameof(JobSeekers), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateJobSeeker(
        Guid userId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _userService.DeactivateJobSeekerAsync(userId, cancellationToken);

        TempData["Success"] = "Job seeker was deactivated successfully.";

        return RedirectToAction(nameof(JobSeekers), new { pageNumber });
    }

    public async Task<IActionResult> Advertisements(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _advertisementService.GetAllAdvertisementsAsync(
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        return View(AdvertisementsViewModel.FromResponseDto(result));
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

    [HttpGet]
    public async Task<IActionResult> Cities(
    int pageNumber = 1,
    CancellationToken cancellationToken = default)
    {
        const int pageSize = 10;

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var cities = await _cityService.GetAllCitiesAsync(
            new TextRequestDto(),
            new PagingRequestDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

        var provinces = await _provinceService.GetAllForSelectAsync(
            cancellationToken);

        ViewBag.Provinces = new SelectList(
            provinces,
            nameof(ProvinceResponseDto.ProvinceId),
            nameof(ProvinceResponseDto.Name));

        return View(CitiesViewModel.FromResponseDto(cities));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCity(
        CreateCityRequestDto model,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The city data is invalid.";

            return RedirectToAction(nameof(Cities), new { pageNumber });
        }

        await _cityService.CreateCityAsync(model, cancellationToken);

        TempData["Success"] = "City was created successfully.";

        return RedirectToAction(nameof(Cities), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCity(
        Guid cityId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _cityService.SoftDeleteAsync(cityId, cancellationToken);

        TempData["Success"] = "City was deleted successfully.";

        return RedirectToAction(nameof(Cities), new { pageNumber });
    }

    [HttpGet]
    public async Task<IActionResult> Provinces(
    int pageNumber = 1,
    CancellationToken cancellationToken = default)
    {
        const int provincePageSize = 10;

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var result = await _provinceService.GetAllProvincesAsync(
            new TextRequestDto(),
            new PagingRequestDto
            {
                PageNumber = pageNumber,
                PageSize = provincePageSize
            },
            cancellationToken);

        return View(ProvincesViewModel.FromResponseDto(result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProvince(
        CreateProvinceRequestDto model,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The province data is invalid.";
            return RedirectToAction(nameof(Provinces), new { pageNumber });
        }

        await _provinceService.CreateProvinceAsync(model, cancellationToken);

        TempData["Success"] = "Province was created successfully.";
        return RedirectToAction(nameof(Provinces), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProvince(
        Guid provinceId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _provinceService.SoftDeleteAsync(provinceId, cancellationToken);

        TempData["Success"] = "Province was deleted successfully.";

        return RedirectToAction(nameof(Provinces), new { pageNumber });
    }

    [HttpGet]
    public async Task<IActionResult> JobCategories(
    int pageNumber = 1,
    CancellationToken cancellationToken = default)
    {
        const int pageSize = 10;

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var result = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

        return View(JobCategoriesViewModel.FromResponseDto(result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateJobCategory(
        CreateJobCategoryRequestDto model,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The job category data is invalid.";

            return RedirectToAction(
                nameof(JobCategories),
                new { pageNumber });
        }

        await _jobCategoryService.CreateJobCategoryAsync(
            model,
            cancellationToken);

        TempData["Success"] = "Job category was created successfully.";

        return RedirectToAction(
            nameof(JobCategories),
            new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteJobCategory(
        Guid jobCategoryId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _jobCategoryService.SoftDeleteAsync(jobCategoryId, cancellationToken);

        TempData["Success"] = "Job category was deleted successfully.";

        return RedirectToAction(
            nameof(JobCategories),
            new { pageNumber });
    }

    [HttpGet]
    public async Task<IActionResult> Skills(
            int pageNumber = 1,
            CancellationToken cancellationToken = default)
    {
        const int pageSize = 10;
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var result = await _skillService.GetAllSkillsAsync(
            new TextRequestDto(),
            new PagingRequestDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

        return View(SkillsViewModel.FromResponseDto(result));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateSkill(
        CreateSkillRequestDto model,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The skill data is invalid. Please check the input.";
            return RedirectToAction(nameof(Skills), new { pageNumber });
        }

        await _skillService.CreateSkillAsync(model, cancellationToken);

        TempData["Success"] = "Skill was created successfully.";

        return RedirectToAction(nameof(Skills), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSkill(
        Guid skillId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _skillService.SoftDeleteAsync(skillId, cancellationToken);

        TempData["Success"] = "Skill was deleted successfully.";

        return RedirectToAction(nameof(Skills), new { pageNumber });
    }

    public async Task<IActionResult> Jobs(
    int pageNumber = 1,
    CancellationToken cancellationToken = default)
    {
        const int pageSize = 10;
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var jobsResult = await _jobService.GetAllJobsAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = pageNumber, PageSize = pageSize },
            cancellationToken);

        var categories = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 1000 },
            cancellationToken);

        ViewBag.JobCategories = new SelectList(categories.Data, nameof(JobCategoryResponseDto.JobCategoryId), nameof(JobCategoryResponseDto.Name));

        return View(JobsViewModel.FromResponseDto(jobsResult));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateJob(
        CreateJobRequestDto model,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The job data is invalid.";
            return RedirectToAction(nameof(Jobs), new { pageNumber });
        }

        await _jobService.CreateJobAsync(model, cancellationToken);
        TempData["Success"] = "Job was created successfully.";

        return RedirectToAction(nameof(Jobs), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteJob(
        Guid jobId,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _jobService.SoftDeleteAsync(jobId, cancellationToken);

        TempData["Success"] = "Job was deleted successfully.";

        return RedirectToAction(nameof(Jobs), new { pageNumber });
    }

    public async Task<IActionResult> EmailTemplates(
    int pageNumber = 1,
    CancellationToken cancellationToken = default)
    {
        const int pageSize = 10;

        pageNumber = pageNumber <= 0 ? 1 : pageNumber;

        var templatesPaginatedResult = await _emailService.GetAllAsync(
            new PagingRequestDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            cancellationToken);

        return View(EmailTemplatesViewModel.FromResponseDto(templatesPaginatedResult));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateTemplate(
        Guid id,
        UpdateTemplateRequestDto model,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "The email template data is invalid.";

            return RedirectToAction(nameof(EmailTemplates), new { pageNumber });
        }

        await _emailService.UpdateTemplateAsync(id, model, cancellationToken);

        TempData["Success"] = "Email template was updated successfully.";

        return RedirectToAction(nameof(EmailTemplates), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateTemplate(
        Guid id,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _emailService.ActivateTemplateAsync(id, cancellationToken);

        TempData["Success"] = "Email template was activated successfully.";

        return RedirectToAction(nameof(EmailTemplates), new { pageNumber });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeactivateTemplate(
        Guid id,
        int pageNumber = 1,
        CancellationToken cancellationToken = default)
    {
        await _emailService.DeactivateTemplateAsync(id, cancellationToken);

        TempData["Success"] = "Email template was deactivated successfully.";

        return RedirectToAction(nameof(EmailTemplates), new { pageNumber });
    }
}
