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

[Authorize(Roles = "JobSeeker")]
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

        try
        {
            await _resumeService.CreateResumeAsync(model, cancellationToken);

            TempData["Success"] = "رزومه با موفقیت ساخته شد.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex) when (ex is AppException)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(Guid resumeId, UploadResumeFileRequestDto model, CancellationToken cancellationToken)
    {
        try
        {
            await _resumeService.UploadResumeFileByResumeIdAsync(resumeId, model, cancellationToken);

            TempData["Success"] = "فایل رزومه آپلود شد.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Download(Guid resumeId, CancellationToken cancellationToken)
    {
        try
        {
            var file = await _resumeService.DownloadResumeFileByResumeIdAsync(resumeId, cancellationToken);

            return File(file.Data, file.ContentType, file.FileName);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFile(Guid resumeId, CancellationToken cancellationToken)
    {
        try
        {
            await _resumeService.DeleteResumeFileByIdAsync(resumeId, cancellationToken);

            TempData["Success"] = "فایل رزومه حذف شد.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
