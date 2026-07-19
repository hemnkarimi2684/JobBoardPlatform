using JobBoardPlatform.Core.Entities.EducationDetailEntity.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.EducationDetailRepo;

public class EducationDetailRepository : GenericRepository<EducationDetail>, IEducationDetailRepository
{
    public EducationDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Guid?> GetEducationDetailUserIdAsync(Guid educationDetailId)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(ed => ed.Id == educationDetailId && !ed.IsDeleted && ed.DeletedAt == null)
                           .Select(ed => ed.UserId)
                           .FirstOrDefaultAsync();
    }

    public async Task<(List<TResult>, int)> GetUserEducationDetailsAsync<TResult>(
        Expression<Func<EducationDetail, TResult>> projection,
        Guid userId,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = Entities
                         .AsNoTracking()
                         .Where(ed => ed.UserId == userId && !ed.IsDeleted && ed.DeletedAt == null);

        var totalDataCount = await query.CountAsync();

        var result = await query
                             .OrderByDescending(b => b.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync();

        return (result, totalDataCount);
    }

    public async Task<bool> UpdateEducationDetailAsync(Guid educationDetailId, UpdateEducationDetail updateEducation)
    {
        var educationDetail = await Entities.FirstOrDefaultAsync(ed => ed.Id == educationDetailId && !ed.IsDeleted && ed.DeletedAt == null);

        if (educationDetail is null)
            return false;

        educationDetail.UpdateEducationDetailInfo(updateEducation);

        return true;
    }

    public Task<bool> UserHasEducationDetailAsync(Guid userId) => AnyAsync(ed => ed.UserId == userId);

}
