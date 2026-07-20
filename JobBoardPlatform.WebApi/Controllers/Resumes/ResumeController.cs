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
    public async Task<IActionResult> CreateResumeAsync([FromBody] CreateResumeRequestDto resumeRequest)
    {
        await _resumeService.CreateResumeAsync(resumeRequest);

        return Ok(Result.Success());
    }

    [HttpGet("by-user/{userId:guid}")]
    [Authorize(Roles = "JobSeeker,Admin,Employer")]
    public async Task<IActionResult> GetResumeDetailAsync([FromRoute] Guid userId)
    {
        var resume = await _resumeService.GetResumeDetailAsync(userId);

        return Ok(Result<ResumeDetailResponseDto>.Success(resume));
    }

    [HttpPatch("{resumeId:guid}/upload-file")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> UploadResumeFileByResumeIdAsync([FromRoute] Guid resumeId, [FromForm] UploadResumeFileRequestDto uploadResumeFile)
    {
        await _resumeService.UploadResumeFileByResumeIdAsync(resumeId, uploadResumeFile);

        return Ok(Result.Success());
    }

    [HttpPatch("by-user/{userId:guid}/upload-file")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> UploadResumeFileByUserIdAsync([FromRoute] Guid userId, [FromForm] UploadResumeFileRequestDto uploadResumeFile)
    {
        await _resumeService.UploadResumeFileByUserIdAsync(userId, uploadResumeFile);

        return Ok(Result.Success());
    }

    [HttpPatch("{resumeId:guid}/download")]
    [Authorize(Roles = "Admin,Employer,JobSeeker")]
    public async Task<IActionResult> DownloadResumeFileAsync([FromRoute] Guid resumeId)
    {
        var result = await _resumeService.DownloadResumeFileByResumeIdAsync(resumeId);

        return File(result.Data, result.ContentType, result.FileName);
    }

    [HttpPatch("by-user/{userId:guid}/download")]
    [Authorize(Roles = "Admin,Employer,JobSeeker")]
    public async Task<IActionResult> DownloadResumeFileByUserIdAsync([FromRoute] Guid userId)
    {
        var result = await _resumeService.DownloadResumeFileByUserIdAsync(userId);

        return File(result.Data, result.ContentType, result.FileName);
    }

    [HttpDelete("{resumeId:guid}")]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> DeleteResumeFileByIdAsync([FromRoute] Guid resumeId)
    {
        await _resumeService.DeleteResumeFileByIdAsync(resumeId);

        return Ok(Result.Success());
    }
}
