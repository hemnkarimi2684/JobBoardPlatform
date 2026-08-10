using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Skills;

[Route("api/skills")]
[ApiController]
[Authorize]
public class SkillController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpGet("by-user/{userId:guid}")]
    public async Task<IActionResult> GetUserSkillsAsync(
        [FromRoute] Guid userId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetUserSkillsAsync(userId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpPost("assign-to-user/{userId:guid}")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> AddSkillsToUserAsync(
        [FromRoute] Guid userId,
        [FromBody] List<Guid> skillsId,
        CancellationToken cancellationToken)
    {
        await _skillService.AddSkillsToUserAsync(userId, skillsId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllSkillsAsync(
        [FromQuery] TextRequestDto textRequestDto,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetAllSkillsAsync(textRequestDto, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{skillId:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSkillByIdAsync(
        [FromRoute] Guid skillId,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetSkillByIdAsync(skillId, cancellationToken);

        return Ok(Result<SkillDetailResponseDto>.Success(result));
    }

    [HttpDelete("remove-from-user/{userId:guid}")]
    [Authorize(Policy = "ActiveJobSeekerOnly")]
    public async Task<IActionResult> RemoveSkillFromUserAsync(
        [FromRoute] Guid userId,
        [FromBody] List<Guid> SkillsId,
        CancellationToken cancellationToken)
    {
        await _skillService.RemoveSkillFromUserAsync(userId, SkillsId, cancellationToken);

        return Ok(Result.Success());
    }
}
