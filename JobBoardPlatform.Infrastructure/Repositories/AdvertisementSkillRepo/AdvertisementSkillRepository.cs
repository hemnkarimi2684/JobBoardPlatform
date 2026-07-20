using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Data;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.AdvertisementSkillRepo;

public class AdvertisementSkillRepository : GenericRepository<AdvertisementSkill>, IAdvertisementSkillRepository
{
    public AdvertisementSkillRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
