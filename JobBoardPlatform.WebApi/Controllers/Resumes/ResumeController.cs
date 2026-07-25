using JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Resumes;

[Route("api/[controller]s")]
[ApiController]
[Authorize]
public class ResumeController : ControllerBase
{
    private readonly IResumeService _resumeService;

    public ResumeController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpPost]
    [Authorize(Roles = "JobSeeker")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateResumeAsync(
        [FromBody] CreateResumeRequestDto resumeRequest,
        CancellationToken cancellationToken)
    {
        await _resumeService.CreateResumeAsync(resumeRequest, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("by-user/{userId:guid}")]
    [Authorize(Roles = "JobSeeker,Admin,Employer")]
    public async Task<IActionResult> GetResumeDetailAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var resume = await _resumeService.GetResumeDetailAsync(userId, cancellationToken);

        return Ok(Result<ResumeDetailResponseDto>.Success(resume));
    }

    [HttpPatch("{resumeId:guid}/upload-file")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> UploadResumeFileByResumeIdAsync(
        [FromRoute] Guid resumeId,
        [FromForm] UploadResumeFileRequestDto uploadResumeFile,
        CancellationToken cancellationToken)
    {
        await _resumeService.UploadResumeFileByResumeIdAsync(resumeId, uploadResumeFile, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("by-user/{userId:guid}/upload-file")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> UploadResumeFileByUserIdAsync(
        [FromRoute] Guid userId,
        [FromForm] UploadResumeFileRequestDto uploadResumeFile,
        CancellationToken cancellationToken)
    {
        await _resumeService.UploadResumeFileByUserIdAsync(userId, uploadResumeFile, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{resumeId:guid}/download")]
    [Authorize(Roles = "Admin,Employer,JobSeeker")]
    public async Task<IActionResult> DownloadResumeFileAsync(
        [FromRoute] Guid resumeId,
        CancellationToken cancellationToken)
    {
        var result = await _resumeService.DownloadResumeFileByResumeIdAsync(resumeId, cancellationToken);

        return File(result.Data, result.ContentType, result.FileName);
    }

    [HttpPatch("by-user/{userId:guid}/download")]
    [Authorize(Roles = "Admin,Employer,JobSeeker")]
    public async Task<IActionResult> DownloadResumeFileByUserIdAsync(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _resumeService.DownloadResumeFileByUserIdAsync(userId, cancellationToken);

        return File(result.Data, result.ContentType, result.FileName);
    }

    [HttpDelete("{resumeId:guid}")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> DeleteResumeFileByIdAsync(
        [FromRoute] Guid resumeId,
        CancellationToken cancellationToken)
    {
        await _resumeService.DeleteResumeFileByIdAsync(resumeId, cancellationToken);

        return Ok(Result.Success());
    }
}
