using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public class UploadUserImageRequestDto
{
    public IFormFile Image { get; set; } = default!;
}
