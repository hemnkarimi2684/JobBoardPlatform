using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;

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
}
