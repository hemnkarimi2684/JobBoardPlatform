using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

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

    /// <summary>
    /// دریافت تمام شرکت ها 
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<CompanyDetailResponseDto>> GetAllCompaniesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت شرکت توسط شناسه اش
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CompanyDetailResponseDto> GetCompanyByIdAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دانلود عکس شرکت 
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadCompanyImageAsync(
        Guid companyId,
        CancellationToken cancellationToken = default);
}
