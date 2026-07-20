using JobBoardPlatform.Core.Entities.AdvertisementEntity.Data;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.AdvertisementRepo;

public class AdvertisementRepository : GenericRepository<Advertisement>, IAdvertisementRepository
{
    public AdvertisementRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TResult?> GetAdvertisementProjectionAsync<TResult>(Expression<Func<Advertisement, TResult>> projection, Guid advertisementId)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(a => a.Id == advertisementId && a.IsActive)
                          .Select(projection)
                          .FirstOrDefaultAsync();
    }

    public async Task<AdvertisementDetail?> GetAdvertisementInfoByIdAsync(Guid advertisementId)
    {
        return await Entities
                        .AsNoTracking()
                        .Where(a => a.Id == advertisementId)
                        .Select(a => new AdvertisementDetail(
                             a.Description,
                             a.MinimumAge,
                             a.MaximumAge,
                             a.MinimumSalary,
                             a.MaximumSalary,
                             a.ExperienceLevel,
                             a.CollaborationType,
                             a.City.Name,
                             a.Company.Name,
                             a.Job.Name,
                             a.Company.AboutUs,
                             a.Company.Industry,
                             a.CreatedAt,
                             a.AdvertisementSkills.Select(s => s.Skill.Name).ToList(),
                             a.Id,
                             a.CityId,
                             a.CompanyId
                             ))
                        .FirstOrDefaultAsync();
    }

    public async Task<Guid?> GetAdvertisementOwnerIdByIdAsync(Guid advertisementId)
    {
        return await Entities
                        .AsNoTracking()
                        .Include(a => a.Company.OwnedByUser)
                        .Where(a => a.Id == advertisementId && !a.IsDeleted && a.DeletedAt == null)
                        .Select(a => a.Company.OwnedByUserId)
                        .FirstOrDefaultAsync();
    }

    public async Task<(List<TResult>, int)> GetAdvertisementsByCompanyAsync<TResult>(Expression<Func<Advertisement, TResult>> projection, Guid companyId, int pageNumber = 1, int pageSize = 10)
    {
        var query = Entities
                         .AsNoTracking()
                         .Where(a => a.CompanyId == companyId && a.IsActive);

        var totalDataCount = await query.CountAsync();

        var result = await query
                             .OrderByDescending(b => b.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync();

        return (result, totalDataCount);
    }

    public async Task<bool> IsAdvertisementExistAsync(Guid advertisementId) => await AnyAsync(a => a.Id == advertisementId);

    public async Task<bool> UpdateAdvertisementInfoAsync(Guid advertisementId, UpdateAdvertisementInfo updateAdvertisementInfo)
    {
        var advertisement = await Entities.FirstOrDefaultAsync(a => a.Id == advertisementId && a.IsActive);

        if (advertisement is null)
            return false;

        advertisement.UpdateAdvertisementInfo(updateAdvertisementInfo);

        return true;
    }

    public async Task<bool> UpdateAdvertisementStatusAsync(Guid advertisementId, Guid? modifiedById, bool isActive)
    {
        var advertisement = await Entities.FirstOrDefaultAsync(a => a.Id == advertisementId && a.IsActive);

        if (advertisement is null)
            return false;

        advertisement.UpdateActiveStatus(modifiedById, isActive);

        return true;
    }
}
