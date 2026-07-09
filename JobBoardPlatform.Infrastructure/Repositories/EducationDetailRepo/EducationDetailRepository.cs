using JobBoardPlatform.Core.Entities.EducationDetailEntity.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;

namespace JobBoardPlatform.Infrastructure.Repositories.EducationDetailRepo;

public class EducationDetailRepository : GenericRepository<EducationDetail>, IEducationDetailRepository
{
    public EducationDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
