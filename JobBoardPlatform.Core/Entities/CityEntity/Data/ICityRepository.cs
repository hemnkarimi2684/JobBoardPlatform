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
    /// <returns></returns>
    Task<bool> IsCityExistAsync(Guid cityId);

    /// <summary>
    /// دریافت شهر های یک استان 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="provinceId"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetProvinceCitiesAsync<TResult>(Expression<Func<City, TResult>> projection,
                                                            Guid provinceId,
                                                            int pageNumber = 1,
                                                            int pageSize = 10);
}
