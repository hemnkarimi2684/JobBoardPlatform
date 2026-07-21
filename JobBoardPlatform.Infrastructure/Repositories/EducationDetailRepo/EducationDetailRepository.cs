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

    public async Task<Guid?> GetEducationDetailUserIdAsync(Guid educationDetailId, CancellationToken cancellationToken)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(ed => ed.Id == educationDetailId)
                           .Select(ed => ed.UserId)
                           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(List<TResult>, int)> GetUserEducationDetailsAsync<TResult>(
        Expression<Func<EducationDetail, TResult>> projection,
        Guid userId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = Entities
                         .AsNoTracking()
                         .Where(ed => ed.UserId == userId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(b => b.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<bool> UpdateEducationDetailAsync(
        Guid educationDetailId,
        CancellationToken cancellationToken,
        UpdateEducationDetail updateEducation)
    {
        var educationDetail = await Entities.FirstOrDefaultAsync(ed => ed.Id == educationDetailId, cancellationToken);

        if (educationDetail is null)
            return false;

        educationDetail.UpdateEducationDetailInfo(updateEducation);

        return true;
    }

    public Task<bool> UserHasEducationDetailAsync(
        Guid userId,
        CancellationToken cancellationToken) => AnyAsync(ed => ed.UserId == userId, cancellationToken);

}
