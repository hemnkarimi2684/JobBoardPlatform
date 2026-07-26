using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class UploadCompanyImageRequestDto
{
    public IFormFile Image { get; set; } = default!;
}
