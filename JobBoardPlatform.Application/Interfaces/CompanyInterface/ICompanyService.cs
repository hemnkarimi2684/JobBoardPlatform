using JobBoardPlatform.Application.Common.Dto.CompanyDto.Command;
using JobBoardPlatform.Application.Common.Dto.CompanyDto.Result;

namespace JobBoardPlatform.Application.Interfaces.CompanyInterface;

public interface ICompanyService
{
    /// <summary>
    /// ساخت شرکت
    /// </summary>
    /// <param name="createCompanyCommand"></param>
    /// <returns></returns>
    Task<bool> CreateCompanyAsync(CreateCompanyCommand createCompanyCommand);

    /// <summary>
    /// دریافت اطلاعات شرکت
    /// </summary>
    /// <param name="ownerId"></param>
    /// <returns></returns>
    Task<CompanyInfoResult> GetCompanyInfoByOwnerIdAsync(Guid ownerId);

    /// <summary>
    /// اپدیت اطلاعات شرکت
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateCompanyIdAsync(Guid companyId, UpdateCompanyInfoCommand updateCommand);
}
