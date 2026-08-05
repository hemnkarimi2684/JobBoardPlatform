using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

public class SkillController : Controller
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    public async Task<IActionResult> Index(string text, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _skillService.GetAllSkillsAsync(
            new TextRequestDto { Text = text },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        ViewBag.Text = text;

        return View(result);
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> MySkills(CancellationToken cancellationToken = default)
    {
        var skills = await _skillService.GetUserSkillsAsync(
            CurrentUserId(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var all = await _skillService.GetAllSkillsAsync(
            new TextRequestDto(),
            new PagingRequestDto { PageNumber = 1, PageSize = 100 },
            cancellationToken);

        var ownedIds = skills.Data.Select(s => s.SkillId).ToHashSet();

        ViewBag.AvailableSkills = all.Data.Where(s => !ownedIds.Contains(s.SkillId)).ToList();

        return View(skills);
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [HttpPost]
    public async Task<IActionResult> Assign(List<Guid> skillsId, CancellationToken cancellationToken)
    {
        await _skillService.AddSkillsToUserAsync(CurrentUserId(), skillsId, cancellationToken);

        TempData["Success"] = "Skills were added successfully.";

        return RedirectToAction(nameof(MySkills));
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
