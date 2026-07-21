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
