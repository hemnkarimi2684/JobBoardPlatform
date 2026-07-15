using JobBoardPlatform.Core.Entities.ResumeEntity.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.ResumeRepo;

public class ResumeRepository : GenericRepository<Resume>, IResumeRepository
{
    public ResumeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TResult?> GetResumeByUserIdAsync<TResult>(Expression<Func<Resume, TResult>> projection, Guid userId)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(r => r.UserId == userId)
                           .Select(projection)
                           .FirstOrDefaultAsync();
    }

    public async Task<bool> IsDuplicateResumeForUserAsync(Guid userId) => await AnyAsync(r => r.UserId == userId);

    public async Task<bool> IsResumeExistAsync(Guid resumeId) => await AnyAsync(r => r.Id == resumeId);

}
