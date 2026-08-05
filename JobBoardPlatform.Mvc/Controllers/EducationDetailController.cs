using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Policy = "ActiveJobSeekerOnly")]
public class EducationDetailController : Controller
{
    private readonly IEducationDetailService _educationDetailService;

    public EducationDetailController(IEducationDetailService educationDetailService)
    {
        _educationDetailService = educationDetailService;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _educationDetailService.GetUserEducationDetailsAsync(
            CurrentUserId(),
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        PopulateDegrees();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEducationDetailRequestDto model, CancellationToken cancellationToken)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
        {
            PopulateDegrees();
            return View(model);
        }

        try
        {
            await _educationDetailService.CreateEducationDetailAsync(model, cancellationToken);

            TempData["Success"] = "Education detail was created successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateDegrees();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _educationDetailService.GetEducationDetailByIdAsync(id, cancellationToken);

            PopulateDegrees();

            return View(new UpdateEducationDetailRequestDto
            {
                CertificateDegree = item.CertificateDegreeName,
                Major = item.Major,
                University = item.University,
                StartDate = item.StartDate,
                CompletionDate = item.CompletionDate,
                Percentage = item.Percentage,
                IsCurrentlyStudying = item.IsCurrentlyStudying
            });
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, UpdateEducationDetailRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            PopulateDegrees();
            return View(model);
        }

        try
        {
            await _educationDetailService.UpdateEducationDetailAsync(id, model, cancellationToken);

            TempData["Success"] = "Education detail was updated successfully.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateDegrees();
            return View(model);
        }
    }

    private void PopulateDegrees()
    {
        ViewBag.CertificateDegrees = Enum.GetValues<CertificateDegree>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
