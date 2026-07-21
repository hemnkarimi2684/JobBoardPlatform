using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class UploadCompanyImageRequestDto
{
    public IFormFile File { get; set; } = default!;
}
