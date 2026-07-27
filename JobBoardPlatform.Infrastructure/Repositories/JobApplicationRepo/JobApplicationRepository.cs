using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Data;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.JobApplicationRepo;

public class JobApplicationRepository : GenericRepository<JobApplication>, IJobApplicationRepository
{
    public JobApplicationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> CheckOwnerHasJobApplicationForResumeAsync(
        Guid resumeId,
        Guid employerId,
        CancellationToken cancellationToken)
    {
        return await Entities
                           .AnyAsync(ja =>
                           ja.ResumeId == resumeId &&
                           ja.Advertisement.Company.OwnedByUserId == employerId,
                           cancellationToken);
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> GetAdvertisementJobApplicationsAsync<TResult>(
         Expression<Func<JobApplication, TResult>> projection,
         Guid advertisementId,
         CancellationToken cancellationToken,
         int pageNumber = 1,
         int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                      .AsNoTracking()
                      .Where(ja => ja.AdvertisementId == advertisementId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                            .OrderByDescending(ja => ja.CreatedAt)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(projection)
                            .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<TResult?> GetJobApplicationByIdAsync<TResult>(
        Expression<Func<JobApplication, TResult>> projection,
        Guid jobApplicationId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(ja => ja.Id == jobApplicationId)
                          .Select(projection)
                          .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> GetJobApplicationsByUserIdAsync<TResult>(
        Expression<Func<JobApplication, TResult>> projection,
        Guid userId, 
        CancellationToken cancellationToken, 
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                      .AsNoTracking()
                      .Where(ja => ja.UserId == userId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                            .OrderByDescending(ja => ja.CreatedAt)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(projection)
                            .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<Guid?> GetJobApplicationUserIdAsync(
        Guid jobApplicationId,
        CancellationToken cancellationToken)
    {
        return await Entities
                         .AsNoTracking()
                         .Where(ja => ja.Id == jobApplicationId)
                         .Select(ja => ja.UserId)
                         .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateJobApplicationAsync(
        Guid advertisementId,
        Guid userId,
        CancellationToken cancellationToken)
                                => await AnyAsync(ja =>
                                    ja.AdvertisementId == advertisementId && ja.UserId == userId,
                                    cancellationToken);

}
