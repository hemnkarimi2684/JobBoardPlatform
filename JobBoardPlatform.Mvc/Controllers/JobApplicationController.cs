using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.JobApplicationDto;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Application.Interfaces.JobApplicationInterface;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.Mvc.Models.JobApplication;
using JobBoardPlatform.Mvc.Models.Resume;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoardPlatform.Mvc.Controllers;

[Authorize]
public class JobApplicationController : Controller
{
    private readonly IJobApplicationService _jobApplicationService;
    private readonly IResumeService _resumeService;
    private readonly IAdvertisementService _advertisementService;

    public JobApplicationController(
        IJobApplicationService jobApplicationService,
        IResumeService resumeService,
        IAdvertisementService advertisementService)
    {
        _jobApplicationService = jobApplicationService;
        _resumeService = resumeService;
        _advertisementService = advertisementService;
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> My(int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobApplicationService.GetJobApplicationsByUserIdAsync(
            CurrentUserId(),
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        return View(JobApplicationMyViewModel.FromResponseDto(result));
    }

    [Authorize(Policy = "ApprovedEmployerOnly")]
    public async Task<IActionResult> ByAdvertisement(Guid advertisementId, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobApplicationService.GetAdvertisementJobApplicationsAsync(
            advertisementId,
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        var advertisement = await _advertisementService.GetAdvertisementProjectionAsync(advertisementId, cancellationToken);

        var status = advertisement?.Status ?? Core.Entities.AdvertisementEntity.Enums.AdvertisementStatus.Open;

        return View(JobApplicationByAdvertisementViewModel.FromResponseDto(result, advertisementId, status));
    }

    [Authorize(Policy = "ApprovedEmployerOnly")]
    [HttpGet]
    public async Task<IActionResult> ApplicantResume(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        var resume = await _jobApplicationService
            .GetApplicantResumeByApplicationIdAsync(
                id,
                CurrentUserId(),
                cancellationToken);

        return View("~/Views/Resume/ApplicantResume.cshtml", ResumeApplicantResumeViewModel.FromResponseDto(resume));
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [HttpPost]
    public async Task<IActionResult> Apply(Guid advertisementId, CancellationToken cancellationToken)
    {
        var userId = CurrentUserId();

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

    [Authorize(Policy = "ApprovedEmployerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(
    Guid id,
    JobApplicationStatus status,
    Guid advertisementId,
    CancellationToken cancellationToken = default)
    {
        await _jobApplicationService.UpdateJobApplicationStatusAsync(
            id,
            status,
            cancellationToken);

        TempData["Success"] = "Application status was updated successfully.";

        return RedirectToAction(nameof(ByAdvertisement), new { advertisementId });
    }

    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        await _jobApplicationService.CancelJobApplicationAsync(id, cancellationToken);

        TempData["Success"] = "Application was canceled successfully.";

        return RedirectToAction(nameof(My));
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : Guid.Empty;
}
