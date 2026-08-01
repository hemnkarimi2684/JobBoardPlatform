using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Data;
using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories.EmailTemplateRepo;

public class EmailTemplateRepository : GenericRepository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<EmailTemplate?> GetByKeyAsync(string templateKey, CancellationToken cancellationToken)
    {
        return await Entities
                         .AsNoTracking()
                         .FirstOrDefaultAsync(et => et.Key == templateKey, cancellationToken);
    }
}
