using JobBoardPlatform.Core.Entities.CityEntity.Entity;
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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistByNameAsync(
        string name, 
        CancellationToken cancellationToken);

    /// <summary>
    /// چک کردن اینکه ایا این کارفرما قبلا شرکت ثبت کرده یا نه
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistForOwnerId(
        Guid ownerId, 
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت اطلاعات شرکت کارفرما
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="ownerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetCompanyByOwnerIdAsync<TResult>(
        Expression<Func<Company, TResult>> projection,
        Guid ownerId,
        CancellationToken cancellationToken);

    /// <summary>
    /// اپدیت اطلاعات شرکت
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="companyInfoUpdate"></param>
    /// <returns></returns>
    Task<bool> UpdateCompanyInfoAsync(
        Guid companyId,
        CancellationToken cancellationToken,
        CompanyInfoUpdate companyInfoUpdate);

    /// <summary>
    /// دریافت شناسه کارفرمای شرکت توسط شناسه شرکت 
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid?> GetCompanyOwnerIdByCompanyIdAsync(
        Guid companyId,
        CancellationToken cancellationToken);

    /// <summary>
    /// ایا شرکت با این شناسه وجود دارد یا نه
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsCompanyExistAsync(
        Guid companyId,
        CancellationToken cancellationToken);

    /// <summary>
    /// دریافت تمام شرکت ها 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="text"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<(List<TResult>, int)> GetAllCompaniesAsync<TResult>(
        Expression<Func<Company, TResult>> projection,
        string? text,
        CancellationToken cancellationToken,
        int pageNumber = 1,
        int pageSize = 10
        );
}
