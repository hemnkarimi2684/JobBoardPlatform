using JobBoardPlatform.Core.Entities.ProvinceEntity.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.ProvinceRepo;

public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
