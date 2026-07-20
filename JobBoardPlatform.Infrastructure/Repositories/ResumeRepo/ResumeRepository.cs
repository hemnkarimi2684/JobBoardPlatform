using JobBoardPlatform.Core.Entities.ResumeEntity.Data;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories.ResumeRepo;

public class ResumeRepository : GenericRepository<Resume>, IResumeRepository
{
    public ResumeRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Resume?> GetResumeByUserIdAsync(Guid userId)
    {
        return await Entities
                         .AsNoTracking()
                         .FirstOrDefaultAsync(r => r.UserId == userId);
    }

    public async Task<Guid?> GetResumeFileIdResumeIdAsync(Guid resumeId)
    {
        return await Entities
                            .AsNoTracking()
                            .Where(r => r.Id == resumeId)
                            .Select(r => r.LastUploadedFileId)
                            .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetResumeFileIdUserIdAsync(Guid userId)
    {
        return await Entities
                            .AsNoTracking()
                            .Where(r => r.UserId == userId)
                            .Select(r => r.LastUploadedFileId)
                            .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetResumeIdByUserIdAsync(Guid userId)
    {
        return await Entities
                        .AsNoTracking()
                        .Where(r => r.UserId == userId)
                        .Select(r => r.UserId)
                        .FirstOrDefaultAsync();
    }

    public async Task<bool> IsDuplicateResumeForUserAsync(Guid userId) => await AnyAsync(r => r.UserId == userId);

    public async Task<bool> IsResumeExistAsync(Guid resumeId) => await AnyAsync(r => r.Id == resumeId);
}
