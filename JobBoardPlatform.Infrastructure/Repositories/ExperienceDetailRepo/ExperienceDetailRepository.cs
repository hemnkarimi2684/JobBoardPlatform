using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Data;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.ExperienceDetailRepo;

public class ExperienceDetailRepository : GenericRepository<ExperienceDetail>, IExperienceDetailRepository
{
    public ExperienceDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
