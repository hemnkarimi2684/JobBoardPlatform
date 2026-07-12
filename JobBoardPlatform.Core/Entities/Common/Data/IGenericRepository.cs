using JobBoardPlatform.Core.Entities.Common.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.Common.Data;

public interface IGenericRepository<T> where T : BaseEntity
{
    /// <summary>
    /// دریافت همه موجودیت ها 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <param name="tracking"></param>
    /// <returns></returns>
    Task<List<TResult>> QueryAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        int page = 1, int pageSize = 10,
        bool tracking = false);

    /// <summary>
    /// دریافت موجدیت ها با شرط خاص 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="selector"></param>
    /// <param name="filter"></param>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="tracking"></param>
    /// <returns></returns>
    Task<List<TResult>> QueryAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> filter,
        int page = 1, int pageSize = 10,
        bool tracking = false);

    /// <summary>
    /// بررسی شرط برای همه موجودیت ها 
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// دریافت یک موجودیت با ایدی 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(int id, bool tracking = false);

    /// <summary>
    /// اضافه کردن موجودیت 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    /// اپدیت موجودیت
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Update(T entity, Guid modifiedById);

    /// <summary>
    /// حذف نرم موجودیت 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> SoftDeleteAsync(int id, Guid deletedById);
}
