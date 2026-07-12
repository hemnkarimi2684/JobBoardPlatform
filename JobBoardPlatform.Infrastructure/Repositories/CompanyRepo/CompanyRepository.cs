using JobBoardPlatform.Core.Entities.CompanyEntity.Data;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.CompanyRepo;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<TResult?> GetCompanyByOwnerIdAsync<TResult>(Expression<Func<Company, TResult>> projection, Guid ownerId)
    {
        return await Entities
                         .AsNoTracking()
                         .Where(c => c.OwnedByUserId == ownerId)
                         .Select(projection)
                         .FirstOrDefaultAsync();
    }

    public async Task<bool> IsCompanyExistByNameAsync(string name) => await AnyAsync(c => c.Name == name);

    public async Task<bool> IsCompanyExistForOwnerId(Guid ownerId) => await AnyAsync(c => c.OwnedByUserId == ownerId);

    public async Task<bool> UpdateCompanyInfoAsync(Guid companyId, CompanyInfoUpdate companyInfoUpdate)
    {
        var company = await Entities.FindAsync(companyId);

        if (company == null)
            return false;

        company.UpdateCompanyInfo(companyInfoUpdate);

        return true;
    }
}
