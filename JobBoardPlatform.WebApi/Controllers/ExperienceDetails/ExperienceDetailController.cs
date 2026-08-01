using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Interfaces.ExperienceDetailInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.Filters;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.ExperienceDetails;

[Route("api/experienceDetail")]
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
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> GetUserExperienceDetailsAsync(
        [FromRoute] Guid userId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _experienceDetailService.GetUserExperienceDetailsAsync(userId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{experienceDetailId:guid}")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> GetExperienceDetailByIdAsync(
        [FromRoute] Guid experienceDetailId,
        CancellationToken cancellationToken)
    {
        var result = await _experienceDetailService.GetExperienceDetailByIdAsync(experienceDetailId, cancellationToken);

        return Ok(Result<ExperienceHistoryResponseDto>.Success(result));
    }

    [HttpPost]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> CreateExperienceDetailAsync(
        [FromBody] CreateExperienceDetailRequestDto createExperience,
        CancellationToken cancellationToken)
    {
        await _experienceDetailService.CreateExperienceDetailAsync(createExperience, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPut("{experienceDetailId:guid}")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    [RequestModelValidationFilter]
    public async Task<IActionResult> UpdateExperienceDetailAsync(
        [FromRoute] Guid experienceDetailId,
        [FromBody] UpdateExperienceDetailRequestDto updateExperience,
        CancellationToken cancellationToken)
    {
        await _experienceDetailService.UpdateExperienceDetailAsync(experienceDetailId, updateExperience, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet("seniority-levels")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSeniorityLevels()
    {
        var result = _experienceDetailService.GetSeniorityLevels();

        return Ok(Result<List<EnumResponseDto>>.Success(result));
    }
}
