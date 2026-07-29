using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
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
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetUserExperienceDetailsAsync<TResult>(
                             Expression<Func<ExperienceDetail, TResult>> projection,
                             Guid userId,
                             CancellationToken cancellationToken,
                             int pageNumber = 1,
                             int pageSize = 10);

    /// <summary>
    /// ویرایش اطلاعات تجربه کاری ثبت شده
    /// </summary>
    /// <param name="experienceDetailId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="updateExperienceDetail"></param>
    /// <returns></returns>
    Task<bool> UpdateExperienceDetailAsync(
        Guid experienceDetailId,
        CancellationToken cancellationToken,
        UpdateExperienceDetail updateExperienceDetail);

    /// <summary>
    /// دریافت شناسه کاربر درای تجربه کار توسط شناسه تجربه کاری 
    /// </summary>
    /// <param name="experienceDetailId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> GetExperienceDetailUserIdAsync(
        Guid experienceDetailId,
        CancellationToken cancellationToken);
}
