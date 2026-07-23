using Azure;
using JobBoardPlatform.Core.Entities.JobEntity.Data;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.JobRepo;

public class JobRepository : GenericRepository<Job>, IJobRepository
{
    public JobRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetAllJobsAsync<TResult>(
        string? text,
        Expression<Func<Job, TResult>> projection,
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

    public async Task<bool> IsDuplicateJobAsync(string jobName, Guid jobCategoryId, CancellationToken cancellationToken)
    {
        return await Entities
                          .AnyAsync(j => j.Name == jobName && j.JobCategoryId == jobCategoryId, cancellationToken);
    }

    public async Task<bool> IsJobExistAsync(
        Guid jobId,
        CancellationToken cancellationToken) => await AnyAsync(j => j.Id == jobId, cancellationToken);

}
