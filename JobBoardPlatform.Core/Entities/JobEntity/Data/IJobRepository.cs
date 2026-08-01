using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.JobEntity.Data;

public interface IJobRepository : IGenericRepository<Job>
{
    /// <summary>
    /// ایا این کار وجود داره یا نه
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsJobExistAsync(
        Guid jobId,
        CancellationToken cancellationToken);

    /// <summary>
    /// ایا این کار در این دسته بندی قبلا ثبت شده یا نه 
    /// </summary>
    /// <param name="jobName"></param>
    /// <param name="jobCategoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateJobAsync(
        string jobName,
        Guid jobCategoryId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تمام شغل ها 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetAllJobsAsync<TResult>(
        string? text,
        Expression<Func<Job, TResult>> projection,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);
}
