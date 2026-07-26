using JobBoardPlatform.Core.Entities.CompanyCityEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.CompanyCityRepo;

public class CompanyCityRepository : GenericRepository<CompanyCity>, ICompanyCityRepository
{
    public CompanyCityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetCityCompaniesAsync<TResult>(
        Expression<Func<CompanyCity, TResult>> projection,
        Guid cityId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                        .AsNoTracking()
                        .Where(cc => cc.CityId == cityId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(us => us.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<bool> IsCompanyExistInCityAsync(
        Guid companyId,
        Guid cityId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AnyAsync(cc => cc.CompanyId == companyId && cc.CityId == cityId, cancellationToken);
    }
}
