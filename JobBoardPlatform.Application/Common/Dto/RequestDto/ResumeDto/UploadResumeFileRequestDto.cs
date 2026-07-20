using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;

public class UploadResumeFileRequestDto
{
    public IFormFile File { get; set; } = default;
}
