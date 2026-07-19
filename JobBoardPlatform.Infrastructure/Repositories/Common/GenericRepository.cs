using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobBoardPlatform.Infrastructure.Repositories.Common;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext DbContext;

    protected readonly DbSet<T> Entities;

    public GenericRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        Entities = DbContext.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await Entities.AddAsync(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        var query = Entities.AsQueryable();

        return await query.AnyAsync(predicate);
    }

    public async Task<Pagination<TResult>> QueryAsync<TResult>(
            Expression<Func<T, TResult>> selector,
            int page = 1, int pageSize = 10,
            bool tracking = false)
    {
        page = page < 0 ? 1 : page;
        pageSize = pageSize < 0 ? 10 : pageSize;

        var query = Entities.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        var result = await query
        .OrderByDescending(b => b.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(selector).ToListAsync();

        return Pagination<TResult>.GetPagination(result, page, pageSize, result.Count());
    }

    public async Task<Pagination<TResult>> QueryAsync<TResult>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>> filter,
        int page = 1, int pageSize = 10,
        bool tracking = false)
    {
        page = page <= 0 ? 1 : page;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var query = Entities.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        var conditionResult = query.Where(filter);

        var totalDataCount = await conditionResult.CountAsync();

        var result = await conditionResult
        .OrderByDescending(b => b.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(selector).ToListAsync();

        return Pagination<TResult>.GetPagination(result, page, pageSize, totalDataCount);
    }

    public async Task<T?> GetByIdAsync(Guid id, bool tracking = false)
    {
        var query = Entities.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        return await Entities.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> SoftDeleteAsync(Guid id, Guid? deletedById)
    {
        var entity = await Entities.FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
            return false;

        entity.SoftDelete(deletedById);

        return DbContext.Entry(entity).State == EntityState.Modified;
    }

    public bool Update(T entity, Guid? modifiedById)
    {
        entity.Update(modifiedById);

        Entities.Update(entity);

        return DbContext.Entry(entity).State == EntityState.Modified;
    }
}

