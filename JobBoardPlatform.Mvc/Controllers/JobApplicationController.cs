using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Common.Exceptions.BaseAppExceptionModel;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.Core.Entities.RoleEntity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize]
public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IResumeService _resumeService;

    public JobApplicationController(IJobApplicationService jobApplicationService, IResumeService resumeService)
    {
        _jobApplicationService = jobApplicationService;
        _resumeService = resumeService;
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> My(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobApplicationService.GetJobApplicationsByUserIdAsync(
            CurrentUserId(),
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        return View(result);
    }

    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> ByAdvertisement(Guid advertisementId, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobApplicationService.GetAdvertisementJobApplicationsAsync(
            advertisementId,
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        ViewBag.AdvertisementId = advertisementId;

        return View(result);
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [HttpPost]
    public async Task<IActionResult> Apply(Guid advertisementId, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();

        try
        {
            var resume = await _resumeService.GetResumeDetailAsync(userId, cancellationToken);

            if (resume.ResumeId is null)
            {
                TempData["Error"] = "Please create a resume before applying for this job.";

                return RedirectToAction("Create", "Resume");
            }

            await _jobApplicationService.CreateJobApplicationAsync(
                new CreateJobApplicationRequestDto
                {
                    ResumeId = resume.ResumeId.Value,
                    AdvertisementId = advertisementId,
                    UserId = userId
                },
                cancellationToken);

            TempData["Success"] = "Your application was submitted successfully.";

            return RedirectToAction("My");
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;

            return RedirectToAction("Details", "Advertisement", new { id = advertisementId });
        }
    }

    [Authorize(Policy = "ApprovedEmployerOnly")]
    [HttpPost]
    public async Task<IActionResult> ChangeStatus(Guid id, JobApplicationStatus status, Guid advertisementId, CancellationToken cancellationToken)
    {
        try
        {
            await _jobApplicationService.UpdateJobApplicationStatusAsync(id, status, cancellationToken);

            TempData["Success"] = "Application status was updated successfully.";
        }
        catch (Exception ex) when (ex is AppException)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(ByAdvertisement), new { advertisementId });
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _jobApplicationService.CancelJobApplicationAsync(id, cancellationToken);
            TempData["Success"] = "Application was canceled successfully.";
        }
        
        catch (Exception ex) when (ex is AppException || ex.GetType().Name == "ValidationException")
        {
            TempData["Error"] = ex.Message;
        }
        catch (Exception)
        {
            TempData["Error"] = "An error occurred while canceling the application. Please try again.";
        }

        return RedirectToAction(nameof(My));
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
