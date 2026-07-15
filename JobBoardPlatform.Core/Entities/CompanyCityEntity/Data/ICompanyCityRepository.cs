using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.CompanyCityEntity.Data;

public interface ICompanyCityRepository : IGenericRepository<CompanyCity>
{
    /// <summary>
    /// دریافت شکرت های در یک شهر 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="cityId"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetCityCompaniesAsync<TResult>(Expression<Func<CompanyCity, TResult>> projection,
                                                              Guid cityId,
                                                              int pageNumber = 1,
                                                              int pageSize = 10);
}
