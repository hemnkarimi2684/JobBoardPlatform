using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;

public class UploadCompanyImageRequestDto
{
    public IFormFile Image { get; set; } = default!;
}
