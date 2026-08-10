using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using JobBoardPlatform.Mvc.Models.EducationDetail;
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

        return View(EducationDetailIndexViewModel.FromResponseDto(result));
    }

    [HttpGet]
    public IActionResult Create()
    {
        PopulateDegrees();
        return View(new EducationDetailCreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(EducationDetailCreateViewModel model, CancellationToken cancellationToken)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
        {
            PopulateDegrees();
            return View(model);
        }

        await _educationDetailService.CreateEducationDetailAsync(model, cancellationToken);

        TempData["Success"] = "Education detail was created successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var item = await _educationDetailService.GetEducationDetailByIdAsync(id, cancellationToken);

        PopulateDegrees();

        return View(EducationDetailEditViewModel.FromResponseDto(item));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, EducationDetailEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            PopulateDegrees();
            return View(model);
        }

        await _educationDetailService.UpdateEducationDetailAsync(id, model, cancellationToken);

        TempData["Success"] = "Education detail was updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _educationDetailService.SoftDeleteAsync(id, cancellationToken);

        TempData["Success"] = "Education detail was deleted successfully.";

        return RedirectToAction(nameof(Index));
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
