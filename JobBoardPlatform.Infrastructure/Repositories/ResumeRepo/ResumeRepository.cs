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

    public async Task<Resume?> GetResumeByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                         .FirstOrDefaultAsync(r => r.UserId == userId, cancellationToken);
    }

    public async Task<Guid?> GetResumeFileIdResumeIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken)
    {
        return await Entities
                            .AsNoTracking()
                            .Where(r => r.Id == resumeId)
                            .Select(r => r.LastUploadedFileId)
                            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetResumeFileIdUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                            .AsNoTracking()
                            .Where(r => r.UserId == userId)
                            .Select(r => r.LastUploadedFileId)
                            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetResumeIdByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await Entities
                        .AsNoTracking()
                        .Where(r => r.UserId == userId)
                        .Select(r => r.UserId)
                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateResumeForUserAsync(
        Guid userId,
        CancellationToken cancellationToken) => await AnyAsync(r => r.UserId == userId, cancellationToken);

    public async Task<bool> IsResumeExistAsync(
        Guid resumeId,
        CancellationToken cancellationToken) => await AnyAsync(r => r.Id == resumeId, cancellationToken);
}
