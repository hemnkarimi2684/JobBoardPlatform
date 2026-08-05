using JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize(Policy = "ActiveJobSeekerOnly")]
public class ResumeController : Controller
{
    private readonly IResumeService _resumeService;

    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var resume = await _resumeService.GetResumeDetailAsync(CurrentUserId(), cancellationToken);

        return View(resume);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateResumeRequestDto model, CancellationToken cancellationToken)
    {
        model.UserId = CurrentUserId();
        ModelState.Remove(nameof(model.UserId));

        if (!ModelState.IsValid)
            return View(model);

        await _resumeService.CreateResumeAsync(model, cancellationToken);

        TempData["Success"] = "Resume was created successfully.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(Guid resumeId, UploadResumeFileRequestDto model, CancellationToken cancellationToken)
    {
        await _resumeService.UploadResumeFileByResumeIdAsync(resumeId, model, cancellationToken);

        TempData["Success"] = "Resume file was uploaded successfully.";

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Download(Guid resumeId, CancellationToken cancellationToken)
    {
        var file = await _resumeService.DownloadResumeFileByResumeIdAsync(resumeId, cancellationToken);

        return File(file.Data, file.ContentType, file.FileName);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFile(Guid resumeId, CancellationToken cancellationToken)
    {
        await _resumeService.DeleteResumeFileByIdAsync(resumeId, cancellationToken);

        TempData["Success"] = "Resume file was deleted successfully.";

        return RedirectToAction(nameof(Index));
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
