using JobBoardPlatform.Core.Entities.SkillEntity.Data;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.SkillRepo;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetAllSkillsAsync<TResult>(Expression<Func<Skill, TResult>> projection,
                                                             string text,
                                                             CancellationToken cancellationToken,
                                                             int pageNumber = 1,
                                                             int pageSize = 10)
    {
        var trimmedText = text.Trim();

        var query = Entities
                         .AsNoTracking()
                         .Where(us => EF.Functions.Like(us.Name, $"%{trimmedText}%"));

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(us => us.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<bool> IsDuplicateSkillAsync(
        string skillName,
        CancellationToken cancellationToken) => await AnyAsync(s => s.Name == skillName, cancellationToken);

}
