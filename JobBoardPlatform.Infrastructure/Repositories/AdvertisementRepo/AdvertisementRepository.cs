using JobBoardPlatform.Core.Entities.AdvertisementEntity.Data;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
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

    public async Task<TResult?> GetAdvertisementProjectionAsync<TResult>(
        Expression<Func<Advertisement, TResult>> projection,
        Guid advertisementId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(a => a.Id == advertisementId && a.IsActive)
                          .Select(projection)
                          .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<AdvertisementDetail?> GetAdvertisementInfoByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken)
    {
        return await Entities
                         .AsNoTracking()
                         .Where(a => a.Id == advertisementId)
                         .Select(a => new AdvertisementDetail
                         {
                             AdvertisementId = a.Id,
                             JobId = a.JobId,
                             Description = a.Description,
                             MinimumAge = a.MinimumAge,
                             MaximumAge = a.MaximumAge,
                             MinimumSalary = a.MinimumSalary,
                             MaximumSalary = a.MaximumSalary,
                             ExperienceLevel = a.ExperienceLevel,
                             CollaborationType = a.CollaborationType,
                             CityName = a.City.Name,
                             CompanyName = a.Company.Name,
                             JobName = a.Job.Name,
                             CompanyAboutUs = a.Company.AboutUs,
                             CompanyJobCategoryId = a.Company.JobCategoryId,
                             CompanyJobCategoryName = a.Company.JobCategory.Name,
                             CreatedAt = a.CreatedAt,
                             Skills = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList(),
                             CityId = a.CityId,
                             CompanyId = a.CompanyId,
                             FeaturedUntil = a.FeaturedUntil,
                             IsFeatured = a.IsFeatured,
                             IsActive = a.IsActive,
                             EmployerUserId = a.Company.OwnedByUserId
                         })
                         .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetAdvertisementOwnerIdByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken)
    {
        return await Entities
                        .AsNoTracking()
                        .Include(a => a.Company.OwnedByUser)
                        .Where(a => a.Id == advertisementId)
                        .Select(a => a.Company.OwnedByUserId)
                        .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> GetAdvertisementsByCompanyAsync<TResult>(
        Expression<Func<Advertisement, TResult>> projection,
        Guid companyId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                         .AsNoTracking()
                         .Where(a => a.CompanyId == companyId && a.IsActive);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(a => a.IsFeatured && a.FeaturedUntil > DateTime.UtcNow)
                             .ThenByDescending(a => a.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<bool> IsAdvertisementExistAsync(
        Guid advertisementId,
        CancellationToken cancellationToken) => await AnyAsync(a => a.Id == advertisementId && a.IsActive, cancellationToken);

    public async Task<bool> UpdateAdvertisementInfoAsync(
        Guid advertisementId,
        CancellationToken cancellationToken,
        UpdateAdvertisementInfo updateAdvertisementInfo)
    {
        var advertisement = await Entities.FirstOrDefaultAsync(a => a.Id == advertisementId && a.IsActive, cancellationToken);

        if (advertisement is null)
            return false;

        advertisement.UpdateAdvertisementInfo(updateAdvertisementInfo);

        return true;
    }

    public async Task<bool> UpdateAdvertisementStatusAsync(
        Guid advertisementId,
        Guid? modifiedById,
        bool isActive,
        CancellationToken cancellationToken)
    {
        var advertisement = await Entities.FirstOrDefaultAsync(a => a.Id == advertisementId, cancellationToken);

        if (advertisement is null)
            return false;

        advertisement.UpdateActiveStatus(modifiedById, isActive);

        return true;
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> FilterAdvertisementsAsync<TResult>(
        AdvertisementQueryFilter filter,
        Expression<Func<Advertisement, TResult>> projection,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber < 1 ? 1 : pageNumber;
        pageSize = pageSize < 1 ? 10 : pageSize;

        var query = Entities
                        .AsNoTracking()
                        .Where(a => a.IsActive);

        if (filter.JobCategoryId.HasValue)
        {
            query = query.Where(a => a.Job.JobCategoryId == filter.JobCategoryId);
        }

        if (filter.MinimumSalary.HasValue)
        {
            query = query.Where(a => a.MinimumSalary >= filter.MinimumSalary.Value);
        }

        if (filter.MaximumSalary.HasValue)
        {
            query = query.Where(a => a.MaximumSalary <= filter.MaximumSalary.Value);
        }

        if (filter.CollabrationType.HasValue)
        {
            query = query.Where(a => a.CollaborationType == filter.CollabrationType);
        }

        if (filter.SkillIds is not null && filter.SkillIds.Count > 0)
        {
            query = query.Where(a => a.AdvertisementSkills.Any(x => filter.SkillIds.Contains(x.SkillId)));
        }

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(a => a.IsFeatured && a.FeaturedUntil > DateTime.UtcNow)
                             .ThenByDescending(a => a.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> SearchAdvertisementsAsync<TResult>(
        string? searchTerm,
        Expression<Func<Advertisement, TResult>> projection,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                        .AsNoTracking()
                        .Where(a => a.IsActive);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();

            query = query
                       .Where(a => EF.Functions.Like(a.Job.Name, $"%{term}%") ||
                                    EF.Functions.Like(a.City.Name, $"%{term}%"));
        }

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(a => a.IsFeatured && a.FeaturedUntil > DateTime.UtcNow)
                             .ThenByDescending(a => a.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<(List<TResult> Items, int TotalDataCount)> GetJobAdvertisementsAsync<TResult>(
        Expression<Func<Advertisement, TResult>> projection,
        Guid jobId,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities
                         .AsNoTracking()
                         .Where(a => a.JobId == jobId);

        var totalDataCount = await query.CountAsync(cancellationToken);

        var result = await query
                             .OrderByDescending(b => b.CreatedAt)
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize)
                             .Select(projection)
                             .ToListAsync(cancellationToken);

        return (result, totalDataCount);
    }

    public async Task<string?> GetAdvertisementOwnerEmailAsync(
        Guid advertisementId,
        CancellationToken cancellationToken)
    {
        return await Entities
                          .AsNoTracking()
                          .Where(a => a.Id == advertisementId)
                          .Select(a => a.Company.OwnedByUser.Email)
                          .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DemoteAdvertisementsAsync()
    {
        var advertisements = Entities
                                 .Where(a => a.IsActive && a.FeaturedUntil != null && a.FeaturedUntil <= DateTime.UtcNow && a.IsFeatured);

        await advertisements.ForEachAsync(a =>
              {
                  a.UpdateFeatured(false, null);
              });
    }
}
