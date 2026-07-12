using JobBoardPlatform.Core.Entities.CompanyCityEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.CompanyCityRepo;

public class CompanyCityRepository : GenericRepository<CompanyCity>, ICompanyCityRepository
{
    public CompanyCityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
}
