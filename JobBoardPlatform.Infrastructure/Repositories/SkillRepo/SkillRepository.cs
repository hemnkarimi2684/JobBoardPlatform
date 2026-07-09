using JobBoardPlatform.Core.Entities.SkillEntity.Data;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.SkillRepo;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
