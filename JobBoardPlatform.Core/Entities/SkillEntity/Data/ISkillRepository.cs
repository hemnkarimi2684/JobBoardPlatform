using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.SkillEntity.Data;

public interface ISkillRepository : IGenericRepository<Skill>
{
    /// <summary>
    /// ایا این مهارت در سیستم وجود دارد 
    /// </summary>
    /// <param name="skillName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDuplicateSkillAsync(
        string skillName,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تمام مهارت ها
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="text"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetAllSkillsAsync<TResult>(Expression<Func<Skill, TResult>> projection,
                                                          string text,
                                                          CancellationToken cancellationToken,
                                                          int pageNumber = 1,
                                                          int pageSize = 10);
}
