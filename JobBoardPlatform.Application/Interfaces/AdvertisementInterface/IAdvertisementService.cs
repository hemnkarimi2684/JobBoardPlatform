using JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Command;
using JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Result;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.AdvertisementInterface;

public interface IAdvertisementService
{
    /// <summary>
    /// ساخت اگهی جدید
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateAdvertisementAsync(CreateAdvertisementCommand createCommand);

    /// <summary>
    /// ویرایش اگهی
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementAsync(Guid advertisementId, UpdateAdvertisementCommand updateCommand);

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
    Task<Pagination<AdvertisementDetailResult>> GetAdvertisementsByCompanyAsync(PagingCommand pagingCommand, Guid companyId);

    /// <summary>
    /// دریافت اطلاعات یک اگهی 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<AdvertisementDetailResult> GetAdvertisementInfoByIdAsync(Guid advertisementId);

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
}
