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

namespace JobBoardPlatform.WebApi.Controllers.EducationDetails;

[Route("api/[controller]")]
[ApiController]
public class EducationDetailController : ControllerBase
{
    private readonly IEducationDetailService _educationDetailService;

    public EducationDetailController(IEducationDetailService educationDetailService)
    {
        _educationDetailService = educationDetailService;
    }

    [HttpGet("{userId:guid}/education-details")]
    [Authorize(Roles = "Admin,JobSeeker")]
    public async Task<IActionResult> GetUserEducationDetailsAsync(
        [FromRoute] Guid userId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _educationDetailService.GetUserEducationDetailsAsync(userId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{educationDetailId:guid}")]
    [Authorize(Roles = "Admin,JobSeeker")]
    public async Task<IActionResult> GetEducationDetailByIdAsync(
        [FromRoute] Guid educationDetailId,
        CancellationToken cancellationToken)
    {
        var result = await _educationDetailService.GetEducationDetailByIdAsync(educationDetailId, cancellationToken);

        return Ok(Result<EducationHistoryResponseDto>.Success(result));
    }

    [HttpPost]
    [RequestModelValidationFilter]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> CreateEducationDetailAsync(
        [FromBody] CreateEducationDetailRequestDto createEducation,
        CancellationToken cancellationToken)
    {
        await _educationDetailService.CreateEducationDetailAsync(createEducation, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPut("{educationDetailId:guid}")]
    [RequestModelValidationFilter]
    [Authorize(Roles = "JobSeeker")]
    public async Task<IActionResult> UpdateEducationDetailAsync(
        [FromRoute] Guid educationDetailId,
        [FromBody] UpdateEducationDetailRequestDto updateEducation,
        CancellationToken cancellationToken)
    {
        await _educationDetailService.UpdateEducationDetailAsync(educationDetailId, updateEducation, cancellationToken);

        return Ok(Result.Success());
    }
}
