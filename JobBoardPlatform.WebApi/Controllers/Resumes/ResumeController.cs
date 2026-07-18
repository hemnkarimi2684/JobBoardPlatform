using JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Application.Interfaces.ResumeInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Resumesک
{
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
        public async Task<IActionResult> CreateResumeAsync([FromBody] CreateResumeRequestDto resumeRequest)
        {
            await _resumeService.CreateResumeAsync(resumeRequest);

            return Ok(Result.Success());
        }

        [HttpGet("by-user/{userId:guid}")]
        [Authorize(Roles = "Employer,Admin")]
        public async Task<IActionResult> GetResumeByUserIdAsync([FromRoute] Guid userId)
        {
            var resume = await _resumeService.GetResumeByUserIdAsync(userId);

            return Ok(Result<ResumeDetailResponseDto>.Success(resume));
        }

        [HttpPatch("{resumeId:guid}/upload-file")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UploadResumeFileAsync([FromRoute] Guid resumeId, [FromForm] UploadResumeFileRequestDto uploadResumeFile)
        {
            await _resumeService.UploadResumeFileAsync(resumeId, uploadResumeFile);

            return Ok(Result.Success());
        }

        [HttpPatch("{resumeId:guid}/download")]
        public async Task<IActionResult> DownloadResumeFileAsync([FromRoute] Guid resumeId)
        {
            var result = await _resumeService.DownloadResumeFileAsync(resumeId);

            return File(result.Data, result.ContentType, result.FileName);
        }
    }
}
