using JobBoardPlatform.Core.Entities.ProvinceEntity.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories.ProvinceRepo;

public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<int> GetProvinceCodeAsync(Guid provinceId, CancellationToken cancellationToken)
    {
        return await Entities
                           .Where(p => p.Id == provinceId)
                           .Select(p => p.ProvinceCode)
                           .FirstOrDefaultAsync(cancellationToken);
    }
}
