using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using System.Linq.Expressions;
using System.Transactions;

namespace JobBoardPlatform.Core.Entities.ProvinceEntity.Data;

public interface IProvinceRepository : IGenericRepository<Province>
{
    /// <summary>
    /// گرفتن کد استان 
    /// </summary>
    /// <param name="provinceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetProvinceCodeAsync(
        Guid provinceId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تمام استان ها 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="text"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetAllProvincesAsync<TResult>(
        Expression<Func<Province, TResult>> projection,
        string? text,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// ایا اسم یا کد تکراری وارد شده 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateNameOrCodeAsync(
        string name,
        int code,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تمام استان ها برای دراپ داون 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<TResult>> GetAllForSelectAsync<TResult>(
        Expression<Func<Province, TResult>> projection,
        CancellationToken cancellationToken);
}
