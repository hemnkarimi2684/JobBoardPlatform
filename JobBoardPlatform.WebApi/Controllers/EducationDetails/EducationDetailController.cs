using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;
using JobBoardPlatform.Application.Interfaces.EducationDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.EducationDetails
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EducationDetailController : ControllerBase
    {
        private readonly IEducationDetailService _educationDetailService;

        public EducationDetailController(IEducationDetailService educationDetailService)
        {
            _educationDetailService = educationDetailService;
        }

        [HttpGet("{userId:guid}/educations-detail")]
        [Authorize(Roles = "Admin,Employer,JobSeeker")]
        public async Task<IActionResult> GetUserEducationDetailsAsync([FromRoute] Guid userId, [FromQuery] PagingRequestDto pagingRequest)
        {
            var result = await _educationDetailService.GetUserEducationDetailsAsync(userId, pagingRequest);

            return Ok(Result<Pagination<UserEducationDetailResponseDto>>.Success(result));
        }

        [HttpPost]
        [RequestModelValidationFilter]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> CreateEducationDetailAsync([FromBody] CreateEducationDetailRequestDto createEducation)
        {
            await _educationDetailService.CreateEducationDetailAsync(createEducation);

            return Ok(Result.Success());
        }

        [HttpPut("{educationDetailId:guid}")]
        [RequestModelValidationFilter]
        [Authorize(Roles = "JobSeeker")]
        public async Task<IActionResult> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetailRequestDto updateEducation)
        {
            await _educationDetailService.UpdateEducationDetailAsync(educationDetailId, updateEducation);

            return Ok(Result.Success());
        }
    }
}
