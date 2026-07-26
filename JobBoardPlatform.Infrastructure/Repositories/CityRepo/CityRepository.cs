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

    public async Task<(List<TResult>, int)> GetAllCitiesAsync<TResult>(
        Expression<Func<City, TResult>> projection,
        string? text,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                        .AsNoTracking()
                        .AsQueryable();

        if (!string.IsNullOrWhiteSpace(text))
        {
            var trimmedText = text.Trim();

            query = query
                       .Where(j => EF.Functions.Like(j.Name, $"%{trimmedText}%"));
        }

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(us => us.Name)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<TResult?> GetCityByIdAsync<TResult>(
        Expression<Func<City, TResult>> projection,
        Guid cityId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(c => c.Id == cityId)
                          .Select(projection)
                          .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(List<TResult>, int)> GetProvinceCitiesAsync<TResult>(
        Expression<Func<City, TResult>> projection,
        Guid provinceId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                        .AsNoTracking()
                        .Where(c => c.ProvinceId == provinceId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                            .OrderByDescending(c => c.Name)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(projection)
                            .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<bool> IsCityExistAsync(
        Guid cityId,
        CancellationToken cancellationToken) => await AnyAsync(c => c.Id == cityId, cancellationToken);

    public async Task<bool> IsDuplicateNameOrCodeAsync(string name, int code, CancellationToken cancellationToken)
    {
        return await Entities
                          .AnyAsync(c => c.Name == name || c.CityCode == code, cancellationToken);
    }
}
