using JobBoardPlatform.Core.Entities.StatusEntity.Data;
using JobBoardPlatform.Core.Entities.StatusEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.StatusRepo;

public class StatusRepository : GenericRepository<Status>, IStatusRepository
{
    public StatusRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
