using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

namespace JobBoardPlatform.Application.Interfaces.CompanyInterface;

public interface ICompanyService
{
    /// <summary>
    /// ساخت شرکت
    /// </summary>
    /// <param name="createCompanyCommand"></param>
    /// <returns></returns>
    Task<Guid> CreateCompanyAsync(CreateCompanyRequestDto createCompanyCommand);

    /// <summary>
    /// دریافت اطلاعات شرکت
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task<CompanyInfoResponseDto> GetCompanyInfoByOwnerIdAsync(Guid ownerId);

    /// <summary>
    /// اپدیت اطلاعات شرکت
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateCompanyIdAsync(Guid companyId, UpdateCompanyInfoRequestDto updateCommand);

    /// <summary>
    /// اپلود عکس شرکت
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="imageRequestDto"></param>
    /// <returns></returns>
    Task UploadCompanyImageAsync(Guid companyId, UploadCompanyImageRequestDto imageRequestDto);
}
