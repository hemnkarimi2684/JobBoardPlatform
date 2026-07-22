using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Skills;

[Route("api/[controller]s")]
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
    [Authorize(Roles = "Admin,Employer,JobSeeker")]
    public async Task<IActionResult> GetUserSkillsAsync(
        [FromRoute] Guid userId,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetUserSkillsAsync(userId, pagingRequest, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateSkillAsync(
       [FromBody] CreateSkillRequestDto skillRequestDto,
        CancellationToken cancellationToken)
    {
        await _skillService.CreateSkillAsync(skillRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPost("assign-to-user/{userId:guid}")]
    [Authorize(Roles = "Admin,JobSeeker")]
    public async Task<IActionResult> AddSkillsToUserAsync(
        [FromRoute] Guid userId,
        [FromBody] List<Guid> skillsId,
        CancellationToken cancellationToken)
    {
        await _skillService.AddSkillsToUserAsync(userId, skillsId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSkillsAsync(
        [FromQuery] string text,
        [FromQuery] PagingRequestDto pagingRequest,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetAllSkillsAsync(text, pagingRequest, cancellationToken);

        return Ok(result);
    }
}
