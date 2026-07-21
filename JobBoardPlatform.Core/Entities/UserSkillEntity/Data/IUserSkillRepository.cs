
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.UserSkillEntity.Data;

public interface IUserSkillRepository : IGenericRepository<UserSkill>
{
    /// <summary>
    /// دریافت مهارت های کاربر
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetUserSkillsAsync<TResult>(Expression<Func<UserSkill, TResult>> projection,
                                              Guid userId,
                                              CancellationToken cancellationToken,
                                              int pageNumber = 1,
                                              int pageSize = 10);
}
