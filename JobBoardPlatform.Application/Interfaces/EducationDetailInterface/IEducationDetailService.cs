using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.EducationDetailInterface;

public interface IEducationDetailService
{
    /// <summary>
    /// دریافت مدرک های تحصیلی کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<EducationHistoryResponseDto>> GetUserEducationDetailsAsync(
        Guid userId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ثبت مدرک تحصیلی 
    /// </summary>
    /// <param name="createCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CreateEducationDetailAsync(
        CreateEducationDetailRequestDto createCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپدیت اطلاعات مدرک تحصیلی ثبت شده
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateEducationDetailAsync(
        Guid educationDetailId,
        UpdateEducationDetailRequestDto updateCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت مدرک تحصیلی توسط شناسه 
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<EducationHistoryResponseDto> GetEducationDetailByIdAsync(
        Guid educationDetailId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام مقطع تحصیلی ها در سیستم 
    /// </summary>
    /// <returns></returns>
    List<EnumResponseDto> GetCertificateDegrees();

    /// <summary>
    /// حذف نرم مدرک تحصیلی
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SoftDeleteAsync(
        Guid educationDetailId,
        CancellationToken cancellationToken = default);
}
