using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.CityEntity.Data;

public interface ICityRepository : IGenericRepository<City>
{
    /// <summary>
    /// چک کردن اینکه ایای شهر وجود دارد یا نه
    /// </summary>
    /// <param name="cityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsCityExistAsync(
        Guid cityId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت شهر های یک استان
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="provinceId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetProvinceCitiesAsync<TResult>(Expression<Func<City, TResult>> projection,
                                                            Guid provinceId,
                                                            CancellationToken cancellationToken,
                                                            int pageNumber = 1,
                                                            int pageSize = 10);

    /// <summary>
    /// چک کردن اینکه اسم یا کد شهر تکراری است یا نه 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateNameOrCodeAsync(
        string name,
        int code,
        CancellationToken cancellationToken);
}
