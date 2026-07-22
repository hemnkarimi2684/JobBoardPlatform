using JobBoardPlatform.Application.Common.Dto.RequestDto.AttachmentDto;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.WebApi.ResultPattern;
using Microsoft.AspNetCore.Mvc;

namespace JobBoardPlatform.WebApi.Controllers.Attachments;

[Route("api/[controller]s")]
[ApiController]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;

    public AttachmentController(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(
        [FromBody] UploadFileRequestDto uploadFile,
        CancellationToken cancellationToken)
    {
        var result = await _attachmentService.UploadAsync(uploadFile.File, uploadFile.AttachmentType, cancellationToken);

        return Ok(Result<Guid>.Success(result));
    }

    [HttpGet("{attachmentId:guid}")]
    public async Task<IActionResult> DownloadAsync(
        [FromRoute] Guid attachmentId,
        CancellationToken cancellationToken)
    {
        var result = await _attachmentService.DownloadAsync(attachmentId, cancellationToken);

        return File(result.Data, result.ContentType, result.FileName);
    }
}
