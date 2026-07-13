using JobBoardPlatform.Core.Entities.Common.Data;
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
}
