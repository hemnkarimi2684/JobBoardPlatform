using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.CompanyEntity.Data;

public interface ICompanyRepository : IGenericRepository<Company>
{
    /// <summary>
    /// چک کردن اینکه ایا شرکت با این اسم وجود دارد 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistByNameAsync(string name);

    /// <summary>
    /// چک کردن اینکه ایا این کارفرما قبلا شرکت ثبت کرده یا نه 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistForOwnerId(Guid ownerId);

    /// <summary>
    /// دریافت اطلاعات شرکت کارفرما
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task<TResult?> GetCompanyByOwnerIdAsync<TResult>(Expression<Func<Company, TResult>> projection, Guid ownerId);

    /// <summary>
    /// اپدیت اطلاعات شرکت 
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="companyInfoUpdate"></param>
    /// <returns></returns>
    Task<bool> UpdateCompanyInfoAsync(Guid companyId, CompanyInfoUpdate companyInfoUpdate);

    /// <summary>
    /// دریافتت شناسه کارفرمای شرکت توسط شناسه شرکت  
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<Guid?> GetCompanyOwnerIdByCompanyIdAsync(Guid companyId);
    
    /// <summary>
    /// ایا شرکت با این شناسه وجود دارد یا نه
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistAsync(Guid companyId);
}
