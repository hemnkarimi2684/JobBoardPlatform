using JobBoardPlatform.Core.Entities.JobEntity.Data;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.JobRepo;

public class JobRepository : GenericRepository<Job>, IJobRepository
{
    public JobRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsJobExistAsync(Guid jobId) => await AnyAsync(j => j.Id == jobId);
    
}
