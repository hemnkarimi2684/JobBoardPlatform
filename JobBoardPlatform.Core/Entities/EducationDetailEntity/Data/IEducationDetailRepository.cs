using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
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
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetUserEducationDetailsAsync<TResult>(
                                            Expression<Func<EducationDetail, TResult>> projection,
                                            Guid userId,
                                            int pageNumber = 1,
                                            int pageSize = 10);

    /// <summary>
    /// ویرایش اطلاعات مدرک تحصیلی موجود
    /// </summary>
    /// <param name="educationDetailId"></param>
    /// <param name="updateEducation"></param>
    /// <returns></returns>
    Task<bool> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetail updateEducation);

    /// <summary>
    /// دریافت شناسه کاربری که این مدرک تحصیلی رو داره توسط شناسه مدرک
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Guid?> GetEducationDetailUserIdAsync(Guid educationDetailId);

    /// <summary>
    /// ایا مدرک تحصیلی دارد یا نه 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<bool> UserHasEducationDetailAsync(Guid userId);
}
