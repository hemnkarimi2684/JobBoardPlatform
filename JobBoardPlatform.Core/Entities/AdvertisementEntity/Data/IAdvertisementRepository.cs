using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using System.Linq.Expressions;
using System.Numerics;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Data;

public interface IAdvertisementRepository : IGenericRepository<Advertisement>
{
    /// <summary>
    /// تغییر دادن وضعیت فعال یا غیرفعال بودن اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="modifiedById"></param>
    /// <param name="isActive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementStatusAsync(
        Guid advertisementId,
        Guid? modifiedById,
        bool isActive,
        CancellationToken cancellationToken);

    /// <summary>
    /// اپدیت اطلاعات اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="updateAdvertisementInfo"></param>
    /// <returns></returns>
    Task<bool> UpdateAdvertisementInfoAsync(
        Guid advertisementId,
        CancellationToken cancellationToken,
        UpdateAdvertisementInfo updateAdvertisementInfo);

    /// <summary>
    /// دریافت اطلاعات یک اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AdvertisementDetail?> GetAdvertisementInfoByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت شناسه کارفرما صاحب اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> GetAdvertisementOwnerIdByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تعداد اگهی های یک شرکت و اگهی ها توسط شناسه شرکت
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetAdvertisementsByCompanyAsync<TResult>(
        Expression<Func<Advertisement, TResult>> projection,
        Guid companyId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// ایا این اگهی موجود است یا نه 
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsAdvertisementExistAsync(
        Guid advertisementId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت اطلاعات مورد نیاز یک اگهی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetAdvertisementProjectionAsync<TResult>(
        Expression<Func<Advertisement, TResult>> projection,
        Guid advertisementId,
        CancellationToken cancellationToken);

    /// <summary>
    /// فیلتر در اگهی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="advertisementQueryFilter"></param>
    /// <param name="projection"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> FilterAdvertisementsAsync<TResult>(
        AdvertisementQueryFilter advertisementQueryFilter,
        Expression<Func<Advertisement, TResult>> projection,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// سرچ در اگهی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="searchTerm"></param>
    /// <param name="projection"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> SearchAdvertisementsAsync<TResult>(
        string searchTerm,
        Expression<Func<Advertisement, TResult>> projection,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// دریافت اگهی های یک شغل 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="jobId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetJobAdvertisementsAsync<TResult>(
        Expression<Func<Advertisement, TResult>> projection,
        Guid jobId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// دریافت ایمیل صاحب اگهی
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> GetAdvertisementOwnerEmailAsync(
        Guid advertisementId,
        CancellationToken cancellationToken);
}

