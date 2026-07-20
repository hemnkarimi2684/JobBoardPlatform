using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Data;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.ExperienceDetailRepo;

public class ExperienceDetailRepository : GenericRepository<ExperienceDetail>, IExperienceDetailRepository
{
    public ExperienceDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid?> GetExperienceDetailUserIdAsync(Guid experienceDetailId)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(ed => ed.Id == experienceDetailId)
                           .Select(ed => ed.UserId)
                           .FirstOrDefaultAsync();
    }

    public async Task<(List<TResult>, int)> GetUserExperienceDetailsAsync<TResult>(Expression<Func<ExperienceDetail, TResult>> projection, Guid userId, int pageNumber = 1, int pageSize = 10)
    {
        var query = Entities
                         .AsNoTracking()
                         .Where(ed => ed.UserId == userId);

        var totalDataCount = await query.CountAsync();

        var result = await query
                             .OrderByDescending(b => b.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync();

        return (result, totalDataCount);
    }

    public async Task<bool> UpdateExperienceDetailAsync(Guid experienceDetailId, UpdateExperienceDetail updateExperienceDetail)
    {
        var experienceDetail = await Entities.FirstOrDefaultAsync(ed => ed.Id == experienceDetailId);

        if (experienceDetail is null)
            return false;

        experienceDetail.UpdateExperienceDetailInfo(updateExperienceDetail);

        return true;
    }
}
