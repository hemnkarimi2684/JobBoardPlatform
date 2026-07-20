using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
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
    /// <returns></returns>
    Task<TResult?> GetJobApplicationByIdAsync<TResult>(Expression<Func<JobApplication, TResult>> projection, Guid jobApplicationId);

    /// <summary>
    /// دریافت درخواست های یک اگهی
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="advertisementId"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetAdvertisementJobApplicationsAsync<TResult>(Expression<Func<JobApplication, TResult>> projection,
                                                                                          Guid advertisementId,
                                                                                          int pageNumber = 1,
                                                                                          int pageSize = 10);

    /// <summary>
    /// ایا درخواست کاری این کاربر برای این اگهی ثبت شده یا نه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="advertisementId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateJobApplicationAsync(Guid advertisementId, Guid userId);

    /// <summary>
    /// دریافت شناسه کاربری که درخواست کار داده
    /// </summary>
    /// <param name="jobApplication"></param>
    /// <returns></returns>
    Task<Guid?> GetJobApplicationUserIdAsync(Guid jobApplicationId);

    /// <summary>
    /// بررسی اینکه برای ایننکه ایا کارفرما متعلق به این درخواست با این رزومه هست یا نه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="currentUserId"></param>
    /// <returns></returns>
    Task<bool> CheckOwnerHasJobApplicationForResumeAsync(Guid resumeId, Guid employerId);
}
