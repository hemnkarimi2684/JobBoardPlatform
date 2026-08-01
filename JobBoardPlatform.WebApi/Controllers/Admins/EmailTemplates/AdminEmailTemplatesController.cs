using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EmailTemplateDto;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Admins.EmailTemplates;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminEmailTemplatesController : ControllerBase
{
    private readonly IEmailService _emailService;

    public AdminEmailTemplatesController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPatch("{templateId:guid}/activate")]
    public async Task<IActionResult> ActivateTemplateAsync(
        [FromRoute] Guid templateId,
        CancellationToken cancellationToken)
    {
        await _emailService.ActivateTemplateAsync(templateId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPatch("{templateId:guid}/deactivate")]
    public async Task<IActionResult> DeactivateTemplateAsync(
        [FromRoute] Guid templateId,
        CancellationToken cancellationToken)
    {
        await _emailService.DeactivateTemplateAsync(templateId, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPut("{templateId:guid}")]
    public async Task<IActionResult> UpdateTemplateAsync(
        [FromRoute] Guid templateId,
        [FromBody] UpdateTemplateRequestDto updateTemplateRequestDto,
        CancellationToken cancellationToken)
    {
        await _emailService.UpdateTemplateAsync(templateId, updateTemplateRequestDto, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken)
    {
        var result = await _emailService.GetAllAsync(pagingRequestDto, cancellationToken);

        return Ok(result);
    }
}
