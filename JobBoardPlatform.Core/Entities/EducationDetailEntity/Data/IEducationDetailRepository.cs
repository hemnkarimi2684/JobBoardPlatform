using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.EducationDetailEntity.Data;

public interface IEducationDetailRepository : IGenericRepository<EducationDetail>
{
    /// <summary>
    /// دریافت مدرک های تحصیلی کاربر 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult> Items, int TotalDataCount)> GetUserEducationDetailsAsync<TResult>(
                                            Expression<Func<EducationDetail, TResult>> projection,
                                            Guid userId,
                                            CancellationToken cancellationToken,
                                            int pageNumber = 1,
                                            int pageSize = 10);

    /// <summary>
    /// ویرایش اطلاعات مدرک تحصیلی موجود
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="updateEducation"></param>
    /// <returns></returns>
    Task<bool> UpdateEducationDetailAsync(
        Guid educationDetailId,
        CancellationToken cancellationToken,
        UpdateEducationDetail updateEducation);

    /// <summary>
    /// دریافت شناسه کاربری که این مدرک تحصیلی رو داره توسط شناسه مدرک
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> GetEducationDetailUserIdAsync(
        Guid educationDetailId,
        CancellationToken cancellationToken);

    /// <summary>
    /// ایا مدرک تحصیلی دارد یا نه 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UserHasEducationDetailAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
