using JobBoardPlatform.Application.Common.Dto.CityDto.Result;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.CompanyDto.Result;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.CityInterface;

public interface ICityService
{
    /// <summary>
    /// دریافت شرکت های در یک شهر 
    /// </summary>
    /// <param name="cityId"></param>
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<CompanyDetailResult>> GetCityCompaniesAsync(Guid cityId, PagingCommand pagingCommand);

    /// <summary>
    /// دریافت شهر های مربوط به یک استان 
    /// </summary>
    /// <param name="provinceId"></param>
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<CityDetailResult>> GetProvinceCitiesAsync(Guid provinceId, PagingCommand pagingCommand);
}
