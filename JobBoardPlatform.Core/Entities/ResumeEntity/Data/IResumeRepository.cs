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
    /// دریافت رزومه کاربر توسط شناسه اش 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<TResult?> GetResumeByUserIdAsync<TResult>(Expression<Func<Resume,TResult>> projection,
                                                   Guid userId);
}
