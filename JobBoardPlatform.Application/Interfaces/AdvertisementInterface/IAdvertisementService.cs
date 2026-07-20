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
    /// <returns></returns>
    Task<bool> CreateAdvertisementAsync(CreateAdvertisementRequestDto createCommand);

    /// <summary>
    /// ویرایش اگهی
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementAsync(Guid advertisementId, UpdateAdvertisementRequestDto updateCommand);

    /// <summary>
    /// حذف نرم اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<bool> SoftDeleteAdvertisementAsync(Guid advertisementId);

    /// <summary>
    /// دریافت اگهی های شرکت 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<Pagination<AdvertisementDetailResponseDto>> GetAdvertisementsByCompanyAsync(PagingRequestDto pagingCommand, Guid companyId);

    /// <summary>
    /// دریافت اطلاعات یک اگهی 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<AdvertisementDetailResponseDto> GetAdvertisementInfoByIdAsync(Guid advertisementId);

    /// <summary>
    /// فعال کردن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<bool> InActivateAdvertisementAsync(Guid advertisementId);

    /// <summary>
    /// غیر فعال کردن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<bool> ActivateAdvertisementAsync(Guid advertisementId);

    /// <summary>
    /// دریافت اطلاعات مورد نیاز یک اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<AdvertisementDisplayResponseDto> GetAdvertisementProjectionAsync(Guid advertisementId);
}
