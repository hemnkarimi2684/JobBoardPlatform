using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using JobBoardPlatform.Mvc.Models.ExperienceDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Policy = "ActiveJobSeekerOnly")]
public class ExperienceDetailController : Controller
{
    private readonly IExperienceDetailService _experienceDetailService;

    public ExperienceDetailController(IExperienceDetailService experienceDetailService)
    {
        _experienceDetailService = experienceDetailService;
    }

    public async Task<IActionResult> Index(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _experienceDetailService.GetUserExperienceDetailsAsync(
            CurrentUserId(),
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        return View(ExperienceDetailIndexViewModel.FromResponseDto(result));
    }

    [HttpGet]
    public IActionResult Create()
    {
        PopulateLevels();
        return View(new ExperienceDetailCreateViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ExperienceDetailCreateViewModel model, CancellationToken cancellationToken)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
        {
            PopulateLevels();
            return View(model);
        }

        await _experienceDetailService.CreateExperienceDetailAsync(model, cancellationToken);

        TempData["Success"] = "Experience detail was created successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        var item = await _experienceDetailService.GetExperienceDetailByIdAsync(id, cancellationToken);

        PopulateLevels();

        return View(ExperienceDetailEditViewModel.FromResponseDto(item));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, ExperienceDetailEditViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            PopulateLevels();
            return View(model);
        }

        await _experienceDetailService.UpdateExperienceDetailAsync(id, model, cancellationToken);

        TempData["Success"] = "Experience detail was updated successfully.";

        return RedirectToAction(nameof(Index));
    }

    private void PopulateLevels()
    {
        ViewBag.SeniorityLevels = Enum.GetValues<SeniorityLevel>()
            .Select(e => new SelectListItem { Value = ((int)e).ToString(), Text = e.ToString() })
            .ToList();
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
