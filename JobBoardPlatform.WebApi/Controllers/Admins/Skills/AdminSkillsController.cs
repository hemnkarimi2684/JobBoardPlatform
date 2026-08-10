using JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;
using JobBoardPlatform.Application.Interfaces.SkillInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.Skills;

[Route("api/admin/skills")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminSkillsController : ControllerBase
{
    private readonly ISkillService _skillService;

    public AdminSkillsController(ISkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSkillAsync(
        [FromBody] CreateSkillRequestDto skillRequestDto,
        CancellationToken cancellationToken)
    {
        await _skillService.CreateSkillAsync(skillRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpDelete("{skillId:guid}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid skillId,
        CancellationToken cancellationToken)
    {
        await _skillService.SoftDeleteAsync(skillId, cancellationToken);

        return Ok(Result.Success());
    }
}
