using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.JobCategoryEntity.Data;

public interface IJobCategoryRepository : IGenericRepository<JobCategory>
{
    /// <summary>
    /// چک کردن اینکه ایا تکراریه یا نه 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateNameAsync(
        string name,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تمام دسته بندی های شغلی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="text"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetAllJobCategoriesAsync<TResult>(
        Expression<Func<JobCategory, TResult>> projection,
        string? text,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// دریافت دسته بندی شغلی توسط شناسه اش 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="jobCategoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetJobCategoryByProjectionAsync<TResult>(
        Expression<Func<JobCategory, TResult>> projection,
        Guid jobCategoryId,
        CancellationToken cancellationToken);

    /// <summary>
    /// ایا این دسته بندی وجود دارد یا نه 
    /// </summary>
    /// <param name="jobCategoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistAsync(
        Guid jobCategoryId,
        CancellationToken cancellationToken);
}
