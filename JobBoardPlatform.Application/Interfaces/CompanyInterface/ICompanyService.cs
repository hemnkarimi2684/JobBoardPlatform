using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

namespace JobBoardPlatform.Application.Interfaces.CompanyInterface;

public interface ICompanyService
{
    /// <summary>
    /// ساخت شرکت
    /// </summary>
    /// <param name="createCompanyCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> CreateCompanyAsync(
        CreateCompanyRequestDto createCompanyCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات شرکت
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CompanyInfoResponseDto> GetCompanyInfoByOwnerIdAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپدیت اطلاعات شرکت
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateCompanyIdAsync(
        Guid companyId,
        UpdateCompanyInfoRequestDto updateCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپلود عکس شرکت
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="imageRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UploadCompanyImageAsync(
        Guid companyId,
        UploadCompanyImageRequestDto imageRequestDto,
        CancellationToken cancellationToken = default);
}
