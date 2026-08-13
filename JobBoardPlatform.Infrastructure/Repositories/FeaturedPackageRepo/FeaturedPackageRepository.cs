using JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Data;
using JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.FeaturedPackageRepo;

public class FeaturedPackageRepository : GenericRepository<FeaturedPackage>, IFeaturedPackageRepository
{
    public FeaturedPackageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<TResult>> GetAllPackagesAsync<TResult>(
        Expression<Func<FeaturedPackage, TResult>> projection,
        CancellationToken cancellationToken)
    {
        return await Entities
            .AsNoTracking()
            .OrderBy(p => p.DurationInDays)
            .Select(projection)
            .ToListAsync(cancellationToken);
    }

    public async Task<FeaturedPackage?> GetByDurationAsync(
        int durationInDays,
        CancellationToken cancellationToken)
    {
        return await Entities
            .FirstOrDefaultAsync(p => p.DurationInDays == durationInDays, cancellationToken);
    }
}
