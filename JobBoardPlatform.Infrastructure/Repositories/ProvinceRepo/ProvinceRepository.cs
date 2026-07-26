using JobBoardPlatform.Core.Entities.ProvinceEntity.Data;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.ProvinceRepo;

public class ProvinceRepository : GenericRepository<Province>, IProvinceRepository
{
    public ProvinceRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetAllProvincesAsync<TResult>(
        Expression<Func<Province, TResult>> projection,
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

    public async Task<int> GetProvinceCodeAsync(Guid provinceId, CancellationToken cancellationToken)
    {
        return await Entities
                           .Where(p => p.Id == provinceId)
                           .Select(p => p.ProvinceCode)
                           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateNameOrCodeAsync(string name, int code, CancellationToken cancellationToken)
    {
        return await Entities
                          .AnyAsync(p => p.Name == name || p.ProvinceCode == code, cancellationToken);
    }
}
