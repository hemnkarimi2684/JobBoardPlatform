using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.CompanyRepo;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<(List<TResult>, int)> GetAllCompaniesAsync<TResult>(
        Expression<Func<Company, TResult>> projection,
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
                       .Where(c => EF.Functions.Like(c.Name, $"%{trimmedText}%"));
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

    public async Task<TResult?> GetCompanyByIdAsync<TResult>(
        Expression<Func<Company, TResult>> projection, 
        Guid companyId, 
        CancellationToken cancellationToken)
    {
        return await Entities
                           .AsNoTracking()
                           .Where(c => c.Id == companyId)
                           .Select(projection)
                           .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TResult?> GetCompanyByOwnerIdAsync<TResult>(
        Expression<Func<Company, TResult>> projection,
        Guid ownerId,
        CancellationToken cancellationToken)
    {
        return await Entities
                         .AsNoTracking()
                         .Where(c => c.OwnedByUserId == ownerId)
                         .Select(projection)
                         .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Guid?> GetCompanyOwnerIdByCompanyIdAsync(
        Guid companyId,
        CancellationToken cancellationToken)
    {
        return await Entities
                         .AsNoTracking()
                         .Where(c => c.Id == companyId)
                         .Select(c => c.Id)
                         .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> IsCompanyExistAsync(
        Guid companyId,
        CancellationToken cancellationToken) => await AnyAsync(c => c.Id == companyId, cancellationToken);

    public async Task<bool> IsCompanyExistByNameAsync(
        string name,
        CancellationToken cancellationToken) => await AnyAsync(c => c.Name == name, cancellationToken);

    public async Task<bool> IsCompanyExistForOwnerId(
        Guid ownerId,
        CancellationToken cancellationToken) => await AnyAsync(c => c.OwnedByUserId == ownerId, cancellationToken);

    public async Task<bool> UpdateCompanyInfoAsync(
        Guid companyId,
        CancellationToken cancellationToken,
        CompanyInfoUpdate companyInfoUpdate)
    {
        var company = await Entities.FirstOrDefaultAsync(c => c.Id == companyId, cancellationToken);

        if (company == null)
            return false;

        company.UpdateCompanyInfo(companyInfoUpdate);

        return true;
    }
}
