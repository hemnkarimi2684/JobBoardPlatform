using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AttachmentDto;

public class UploadFileRequestDto
{
    public IFormFile File { get; set; } = default!;
}
