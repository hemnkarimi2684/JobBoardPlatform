using JobBoardPlatform.Core.Entities.CityEntity.Data;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.CityRepo;

public class CityRepository : GenericRepository<City>, ICityRepository
{
    public CityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetProvinceCitiesAsync<TResult>(Expression<Func<City, TResult>> projection, Guid provinceId, int pageNumber = 1, int pageSize = 10)
    {
        var query = Entities
                        .AsNoTracking()
                        .Where(c => c.ProvinceId == provinceId && !c.IsDeleted && c.DeletedAt == null);

        var totalDataCount = await query.CountAsync();

        var result = await query
                            .OrderByDescending(c => c.Name)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(projection)
                            .ToListAsync();

        return (result, totalDataCount);
    }

    public async Task<bool> IsCityExistAsync(Guid cityId) => await AnyAsync(c => c.Id == cityId);

}
