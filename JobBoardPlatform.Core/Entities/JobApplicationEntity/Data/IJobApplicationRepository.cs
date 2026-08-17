using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.JobApplicationEntity.Data;

public interface IJobApplicationRepository : IGenericRepository<JobApplication>
{
    /// <summary>
    /// دریافت درخواست توسط شناسه اش
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="jobApplicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetJobApplicationByIdAsync<TResult>(
        Expression<Func<JobApplication, TResult>> projection,
        Guid jobApplicationId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت درخواست های یک اگهی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="advertisementId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetAdvertisementJobApplicationsAsync<TResult>(
        Expression<Func<JobApplication, TResult>> projection,
        Guid advertisementId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// ایا درخواست کاری این کاربر برای این اگهی ثبت شده یا نه
    /// </summary>
    /// <param name="advertisementId"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateJobApplicationAsync(
        Guid advertisementId,
        Guid userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت شناسه کاربری که درخواست کار داده
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> GetJobApplicationUserIdAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken);

    /// <summary>
    /// بررسی اینکه برای اینکه ایا کارفرما متعلق به این درخواست با این رزومه هست یا نه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="employerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CheckOwnerHasJobApplicationForResumeAsync(
        Guid resumeId,
        Guid employerId,
        CancellationToken cancellationToken);

    Task<(List<TResult> Items, int TotalDataCount)> GetJobApplicationsByUserIdAsync<TResult>(
        Expression<Func<JobApplication, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10);

    /// <summary>
    /// بررسی اینکه کافرمایی که برای دیدن درخواست کاری درخواست داده ایا کاربر لاگین شده اس یا یکی دیگه اس میخواد یه چیز دیگه ای ببینه
    /// </summary>
    /// <param name="jobApplicationId"></param>
    /// <param name="employerUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> GetApplicantUserIdIfEmployerOwnsApplicationAsync(
    Guid jobApplicationId,
    Guid employerUserId,
    CancellationToken cancellationToken);
}
