using JobBoardPlatform.Core.Entities.UserSkillEntity.Data;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.UserSkillRepo;

public class UserSkillRepository : GenericRepository<UserSkill>, IUserSkillRepository
{
    public UserSkillRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
