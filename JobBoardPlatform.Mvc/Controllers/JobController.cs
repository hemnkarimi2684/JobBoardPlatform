using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.JobInterface;
using JobBoardPlatform.Mvc.Models.Job;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.Mvc.Controllers;

public class JobController : Controller
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    public async Task<IActionResult> Index(string text, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobService.GetAllJobsAsync(
            new TextRequestDto { Text = text },
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 20 },
            cancellationToken);

        ViewBag.Text = text;

        return View(JobIndexViewModel.FromResponseDto(result));
    }

    public async Task<IActionResult> Details(Guid id, int pageNumber = 1, CancellationToken cancellationToken = default)
    {
        var result = await _jobService.GetJobAdvertisementsAsync(
            id,
            new PagingRequestDto { PageNumber = pageNumber, PageSize = 10 },
            cancellationToken);

        return View(JobDetailsViewModel.FromResponseDto(result));
    }
}
