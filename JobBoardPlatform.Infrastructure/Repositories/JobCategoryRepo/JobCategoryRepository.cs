using JobBoardPlatform.Core.Entities.JobCategoryEntity.Data;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.JobCategoryRepo;

public class JobCategoryRepository : GenericRepository<JobCategory>, IJobCategoryRepository
{
    public JobCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistAsync(Guid jobCategoryId, CancellationToken cancellationToken)
    {
        return await Entities
                          .AnyAsync(jc => jc.Id == jobCategoryId, cancellationToken);
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> GetAllJobCategoriesAsync<TResult>(
        Expression<Func<JobCategory, TResult>> projection,
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
                       .Where(jc => EF.Functions.Like(jc.Name, $"%{trimmedText}%"));
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

    public async Task<TResult?> GetJobCategoryByProjectionAsync<TResult>(
        Expression<Func<JobCategory, TResult>> projection,
        Guid jobCategoryId, 
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(jc => jc.Id == jobCategoryId)
                          .Select(projection)
                          .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsDuplicateNameAsync(string name, CancellationToken cancellationToken)
    {
        return Entities
                    .AnyAsync(jc => jc.Name == name, cancellationToken);
    }
}
