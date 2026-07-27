using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Dto;
using System.Linq.Expressions;

namespace JobBoardPlatform.Application.Interfaces.AdvertisementInterface;

public interface IAdvertisementService
{
    /// <summary>
    /// ساخت اگهی جدید
    /// </summary>
    /// <param name="createCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CreateAdvertisementAsync(
        CreateAdvertisementRequestDto createCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ویرایش اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementAsync(
        Guid advertisementId,
        UpdateAdvertisementRequestDto updateCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف نرم اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> SoftDeleteAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اگهی های شرکت 
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<AdvertisementDetailResponseDto>> GetAdvertisementsByCompanyAsync(
        PagingRequestDto pagingCommand,
        Guid companyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات یک اگهی 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AdvertisementDetailResponseDto> GetAdvertisementInfoByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// فعال کردن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> InActivateAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// غیر فعال کردن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ActivateAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات مورد نیاز یک اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AdvertisementDisplayResponseDto> GetAdvertisementProjectionAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام اگهی های فعال
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<AdvertisementDetailResponseDto>> GetActiveAdvertisementsAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// جستجو در اگهی
    /// </summary>
    /// <param name="searchDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<AdvertisementDetailResponseDto>> SearchAdvertisementsAsync(
        AdvertisementSearchRequestDto searchDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// فیلتر در اگهی
    /// </summary>
    /// <param name="filterDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<AdvertisementDetailResponseDto>> FilterAdvertisementsAsync(
        AdvertisementFilterRequestDto filterDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام اگهی های فعال و غیر فعال 
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<AdvertisementDetailResponseDto>> GetAllAdvertisementsAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// ویژه کردن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task PromoteAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// عادی کردن یک اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DemoteAdvertisementAsync(
        Guid advertisementId, 
        CancellationToken cancellationToken = default);
}
