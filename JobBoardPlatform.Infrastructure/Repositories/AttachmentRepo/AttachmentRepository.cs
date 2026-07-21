using JobBoardPlatform.Core.Entities.AttachmentEntity.Data;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.AttachmentRepo;

public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TResult?> GetAttachmentByIdAsync<TResult>(
        Expression<Func<Attachment, TResult>> projection,
        Guid attachmentId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(a => a.Id == attachmentId)
                          .Select(projection)
                          .FirstOrDefaultAsync(cancellationToken);

    }

    public async Task<bool> HardDeleteAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken)
    {
        var attchment = await Entities.FindAsync(attachmentId, cancellationToken);

        if (attchment == null)
            return false;

        var result = Entities.Remove(attchment);

        return result.State == EntityState.Deleted;
    }
}
