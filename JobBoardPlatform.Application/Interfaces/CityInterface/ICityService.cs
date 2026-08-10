using JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.CityInterface;

public interface ICityService
{
    /// <summary>
    /// ساخت شهر
    /// </summary>
    /// <param name="cityRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateCityAsync(
        CreateCityRequestDto cityRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت شرکت های در یک شهر 
    /// </summary>
    /// <param name="cityId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<CompanyListItemResponseDto>> GetCityCompaniesAsync(
        Guid cityId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت شهر های مربوط به یک استان 
    /// </summary>
    /// <param name="provinceId"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<CityDetailResponseDto>> GetProvinceCitiesAsync(
        Guid provinceId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام شهرها
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<CityDetailResponseDto>> GetAllCitiesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت شهر توسط ایدی
    /// </summary>
    /// <param name="cityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CityDetailResponseDto> GetCityByIdAsync(
        Guid cityId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام شهر ها برای دراپ داون 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CityDetailResponseDto>> GetAllForSelectAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف نرم شهر 
    /// </summary>
    /// <param name="cityId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SoftDeleteAsync(
        Guid cityId, 
        CancellationToken cancellationToken = default);
}
