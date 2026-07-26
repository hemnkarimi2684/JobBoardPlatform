using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.CompanyCityEntity.Data;

public interface ICompanyCityRepository : IGenericRepository<CompanyCity>
{
    /// <summary>
    /// دریافت شرکت های یک شهر 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="cityId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetCityCompaniesAsync<TResult>(Expression<Func<CompanyCity, TResult>> projection,
                                                              Guid cityId,
                                                              CancellationToken cancellationToken,
                                                              int pageNumber = 1,
                                                              int pageSize = 10);
    
    /// <summary>
    /// ایا این شرکت در این شهر مورد نظر وجود دارد یا نه 
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="cityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistInCityAsync(
        Guid companyId,
        Guid cityId,
        CancellationToken cancellationToken);
}
