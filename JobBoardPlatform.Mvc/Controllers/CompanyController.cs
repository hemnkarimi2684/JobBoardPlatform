using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.CompanyInterface;
using JobBoardPlatform.Application.Interfaces.JobCategoryInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

public class CompanyController : Controller
{
    private readonly ICompanyService _companyService;
    private readonly IUserService _userService;
    private readonly IJobCategoryService _jobCategoryService;

    public CompanyController(ICompanyService companyService, IUserService userService, IJobCategoryService jobCategoryService)
    {
        _companyService = companyService;
        _userService = userService;
        _jobCategoryService = jobCategoryService;
    }

    public async Task<IActionResult> Index(string text, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _companyService.GetAllCompaniesAsync(
            new TextRequestDto { Text = text },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        ViewBag.Text = text;

        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var company = await _companyService.GetCompanyByIdAsync(id, cancellationToken);

            return View(company);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Employer")]
    public async Task<IActionResult> MyCompany(CancellationToken cancellationToken = default)
    {
        var employer = await _userService.GetEmployerWithCompanyAsync(CurrentUserId(), cancellationToken);

        return View(employer);
    }

    [Authorize(Roles = "Employer")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var company = await _companyService.GetCompanyByIdAsync(id, cancellationToken);

            await PopulateSelectListsAsync(cancellationToken);

            return View(new UpdateCompanyInfoRequestDto
            {
                Name = company.Name,
                YearOfEstablishment = company.YearOfEstablishment,
                AboutUs = company.AboutUs,
                WebSiteAddress = company.WebSiteAddress,
                OwnershipType = company.OwnershipType,
                CompanySize = company.CompanySize,
                JobCategoryId = company.JobCategoryId,
                ActivityType = company.ActivityType
            });
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, UpdateCompanyInfoRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }

        try
        {
            await _companyService.UpdateCompanyIdAsync(id, model, cancellationToken);

            TempData["Success"] = "اطلاعات شرکت با موفقیت ویرایش شد.";

            return RedirectToAction(nameof(MyCompany));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            await PopulateSelectListsAsync(cancellationToken);
            return View(model);
        }
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> UploadImage(Guid id, UploadCompanyImageRequestDto model, CancellationToken cancellationToken)
    {
        try
        {
            await _companyService.UploadCompanyImageAsync(id, model, cancellationToken);

            TempData["Success"] = "لوگوی شرکت آپلود شد.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(MyCompany));
    }

    [Authorize(Roles = "Employer")]
    [HttpPost]
    public async Task<IActionResult> DeleteImage(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _companyService.DeleteCompanyImageAsync(id, cancellationToken);

            TempData["Success"] = "لوگوی شرکت حذف شد.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(MyCompany));
    }

    public async Task<IActionResult> DownloadImage(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var image = await _companyService.DownloadCompanyImageAsync(id, cancellationToken);

            return File(image.Data, image.ContentType, image.FileName);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;

    private async Task PopulateSelectListsAsync(CancellationToken cancellationToken)
    {
        var categories = await _jobCategoryService.GetAllJobCategoriesAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        ViewBag.JobCategories = new SelectList(categories.Data, nameof(JobCategoryResponseDto.JobCategoryId), nameof(JobCategoryResponseDto.Name));
        ViewBag.OwnershipTypes = Enum.GetValues<OwnershipType>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
        ViewBag.CompanySizes = Enum.GetValues<CompanySizeEnum>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
    }
}
