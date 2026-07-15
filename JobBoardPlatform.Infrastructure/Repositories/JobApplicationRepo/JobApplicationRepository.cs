using JobBoardPlatform.Core.Entities.JobApplicationEntity.Data;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Dto;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
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

    public async Task<(List<TResult>, int)> GetAdvertisementJobApplicationsAsync<TResult>(Expression<Func<JobApplication, TResult>> projection,
                                                                                          Guid advertisementId,
                                                                                          int pageNumber = 1,
                                                                                          int pageSize = 10)
    {
        var query = Entities
                      .AsNoTracking()
                      .Where(ja => ja.AdvertisementId == advertisementId);

        var totalDataCount = await query.CountAsync();

        var result = await query
                            .OrderByDescending(ja => ja.CreatedAt)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .Select(projection)
                            .ToListAsync();

        return (result, totalDataCount);
    }

    public async Task<TResult?> GetJobApplicationByIdAsync<TResult>(Expression<Func<JobApplication, TResult>> projection, Guid jobApplicationId)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(ja => ja.Id == jobApplicationId)
                          .Select(projection)
                          .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetJobApplicationUserIdAsync(Guid jobApplicationId)
    {
        return await Entities
                         .AsNoTracking()
                         .Where(ja => ja.Id == jobApplicationId)
                         .Select(ja => ja.UserId)
                         .FirstOrDefaultAsync();
    }

    public async Task<bool> IsDuplicateJobApplicationAsync(Guid advertisementId, Guid userId)
                                => await AnyAsync(ja => ja.AdvertisementId == advertisementId && ja.UserId == userId);

}
