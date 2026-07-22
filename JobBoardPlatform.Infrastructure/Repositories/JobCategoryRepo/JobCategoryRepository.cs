using JobBoardPlatform.Core.Entities.JobCategoryEntity.Data;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.JobCategoryRepo;

public class JobCategoryRepository : GenericRepository<JobCategory>, IJobCategoryRepository
{
    public JobCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
