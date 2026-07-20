using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Data;

public interface IExperienceDetailRepository : IGenericRepository<ExperienceDetail>
{
    /// <summary>
    /// دریافت تجربه کاری های یک کاربر توسط شناسه ان 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="userId"></param>
    /// <param name="pageNumber"></param>
    /// <param name="PageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetUserExperienceDetailsAsync<TResult>(
                             Expression<Func<ExperienceDetail, TResult>> projection,
                             Guid userId,
                             int pageNumber = 1,
                             int pageSize = 10);

    /// <summary>
    /// ویرایش اطلاعات تجربه کاری ثبت شده 
    /// </summary>
    /// <param name="experienceDetailId"></param>
    /// <param name="updateExperienceDetail"></param>
    /// <returns></returns>
    Task<bool> UpdateExperienceDetailAsync(Guid experienceDetailId, UpdateExperienceDetail updateExperienceDetail);

    /// <summary>
    /// دریافت شناسه کاربر درای تجربه کار توسط شناسه تجربه کاری 
    /// </summary>
    /// <param name="experienceDetailId"></param>
    /// <returns></returns>
    Task<Guid?> GetExperienceDetailUserIdAsync(Guid experienceDetailId);
}
