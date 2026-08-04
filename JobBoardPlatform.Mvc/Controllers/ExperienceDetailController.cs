using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Roles = "JobSeeker")]
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

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        PopulateLevels();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExperienceDetailRequestDto model, CancellationToken cancellationToken)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
        {
            PopulateLevels();
            return View(model);
        }

        try
        {
            await _experienceDetailService.CreateExperienceDetailAsync(model, cancellationToken);

            TempData["Success"] = "سوابق کاری ثبت شد.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateLevels();
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _experienceDetailService.GetExperienceDetailByIdAsync(id, cancellationToken);

            PopulateLevels();

            return View(new UpdateExperienceDetailRequestDto
            {
                LastJobTitle = item.LastJobTitle,
                SeniorityLevel = item.SeniorityLevel,
                JobCategory = item.JobCategory,
                City = item.City,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                IsCurrentJob = item.IsCurrentJob
            });
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, UpdateExperienceDetailRequestDto model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            PopulateLevels();
            return View(model);
        }

        try
        {
            await _experienceDetailService.UpdateExperienceDetailAsync(id, model, cancellationToken);

            TempData["Success"] = "سوابق کاری ویرایش شد.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateLevels();
            return View(model);
        }
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
