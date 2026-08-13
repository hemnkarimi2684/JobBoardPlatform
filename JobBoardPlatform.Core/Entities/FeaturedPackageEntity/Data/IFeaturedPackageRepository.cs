using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Data;

public interface IFeaturedPackageRepository : IGenericRepository<FeaturedPackage>
{
    /// <summary>
    /// دریافت تمام بسته های ویژه اگهی
    /// </summary>
    Task<List<TResult>> GetAllPackagesAsync<TResult>(
        Expression<Func<FeaturedPackage, TResult>> projection,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت یک بسته ویژه بر اساس مدت زمان
    /// </summary>
    Task<FeaturedPackage?> GetByDurationAsync(
        int durationInDays,
        CancellationToken cancellationToken);
}
