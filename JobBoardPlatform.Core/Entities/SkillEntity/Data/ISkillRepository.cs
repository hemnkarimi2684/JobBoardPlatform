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
    /// <returns></returns>
    Task<bool> IsDuplicateSkillAsync(string skillName);

    /// <summary>
    /// دریافت تمام مهارت ها 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetAllSkillsAsync<TResult>(Expression<Func<Skill, TResult>> projection,
                                                          string text,
                                                          int pageNumber = 1,
                                                          int pageSize = 10);
}
