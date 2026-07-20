using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.ResumeEntity.Data;

public interface IResumeRepository : IGenericRepository<Resume>
{
    /// <summary>
    /// ایا این رزومه وجود دارد یا نه 
    /// </summary>
    /// <param name="resumeId"></param>
    /// <returns></returns>
    Task<bool> IsResumeExistAsync(Guid resumeId);

    /// <summary>
    /// ایا برای این کاربر قبلا رزومه ثبت شده یا نه 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateResumeForUserAsync(Guid userId);

    /// <summary>
    /// دریافت ایدی فایل اپلود شده رزومه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <returns></returns>
    Task<Guid?> GetResumeFileIdResumeIdAsync(Guid resumeId);

    /// <summary>
    /// دریافت رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Resume?> GetResumeByUserIdAsync(Guid userId);

    /// <summary>
    /// دریافت شناسه فایل رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="resumeId"></param>
    /// <returns></returns>
    Task<Guid?> GetResumeFileIdUserIdAsync(Guid userId);

    /// <summary>
    /// دریافت شناسه رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Guid?> GetResumeIdByUserIdAsync(Guid userId);
}
