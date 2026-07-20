using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.ExperienceDetails
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExperienceDetailController : ControllerBase
    {
        private readonly IExperienceDetailService _experienceDetailService;

        public ExperienceDetailController(IExperienceDetailService experienceDetailService)
        {
            _experienceDetailService = experienceDetailService;
        }

        [HttpGet("{userId:guid}/experience-details")]
        [Authorize(Roles = "Admin,Employer,JobSeeker")]
        [RequestModelValidationFilter]
        public async Task<IActionResult> GetUserExperienceDetailsAsync([FromRoute] Guid userId, [FromQuery] PagingRequestDto pagingRequest)
        {
            var result = await _experienceDetailService.GetUserExperienceDetailsAsync(userId, pagingRequest);

            return Ok(Result<Pagination<UserExperienceDetailResponseDto>>.Success(result));
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        [RequestModelValidationFilter]
        public async Task<IActionResult> CreateExperienceDetailAsync([FromBody] CreateExperienceDetailRequestDto createExperience)
        {
            await _experienceDetailService.CreateExperienceDetailAsync(createExperience);

            return Ok(Result.Success());
        }

        [HttpPut("{experienceDetailId:guid}")]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateExperienceDetailAsync([FromRoute] Guid experienceDetailId, [FromBody] UpdateExperienceDetailRequestDto updateExperience)
        {
            await _experienceDetailService.UpdateExperienceDetailAsync(experienceDetailId, updateExperience);

            return Ok(Result.Success());
        }
    }
}
