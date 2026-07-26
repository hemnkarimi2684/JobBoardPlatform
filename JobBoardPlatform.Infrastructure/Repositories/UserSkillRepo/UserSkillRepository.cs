using JobBoardPlatform.Core.Entities.UserSkillEntity.Data;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.UserSkillRepo;

public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkillRepository
{
    public UserSkillRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetUserSkillsAsync<TResult>(
        Expression<Func<UserSkill, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                        .AsNoTracking()
                        .Where(us => us.UserId == userId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(us => us.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }
}
