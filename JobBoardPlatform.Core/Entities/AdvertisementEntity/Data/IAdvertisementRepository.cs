using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using System.Linq.Expressions;
using System.Numerics;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Data;

public interface IAdvertisementRepository : IGenericRepository<Advertisement>
{
    /// <summary>
    /// تغییر دادن وضعیت فعال یا غیرفعال بودن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementStatusAsync(Guid advertisementId, Guid? modifiedById, bool isActive);

    /// <summary>
    /// اپدیت اطلاعات اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="updateAdvertisementInfo"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementInfoAsync(Guid advertisementId, UpdateAdvertisementInfo updateAdvertisementInfo);

    /// <summary>
    /// دریافت اطلاعات یک اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<AdvertisementDetail?> GetAdvertisementInfoByIdAsync(Guid advertisementId);

    /// <summary>
    /// دریافت شناسه کارفرما صاحب اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<Guid?> GetAdvertisementOwnerIdByIdAsync(Guid advertisementId);

    /// <summary>
    /// دریافت تعداد اگهی های یک شرکت و اگهی ها توسط شناسه شرکت
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="CompanyId"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetAdvertisementsByCompanyAsync<TResult>(
                                              Expression<Func<Advertisement, TResult>> projection,
                                              Guid CompanyId,
                                              int pageNumber = 1,
                                              int pageSize = 10);

    /// <summary>
    /// ایا این اگهی موجود است یا نه 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<bool> IsAdvertisementExistAsync(Guid advertisementId);

    /// <summary>
    /// دریافت اطلاعات مورد نیاز یک اگهی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="advertisementId"></param>
    /// <returns></returns>
    Task<TResult?> GetAdvertisementProjectionAsync<TResult>(Expression<Func<Advertisement, TResult>> projection, Guid advertisementId);
}
