using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
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
    Task<Pagination<CompanyDetailResponseDto>> GetCityCompaniesAsync(Guid cityId, PagingRequestDto pagingCommand);

    /// <summary>
    /// دریافت شهر های مربوط به یک استان 
    /// </summary>
    /// <param name="provinceId"></param>
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<CityDetailResponseDto>> GetProvinceCitiesAsync(Guid provinceId, PagingRequestDto pagingCommand);
}
