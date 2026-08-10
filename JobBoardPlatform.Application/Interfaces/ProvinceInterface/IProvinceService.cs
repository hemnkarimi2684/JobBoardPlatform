using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.ProvinceDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ProvinceDto;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Application.Interfaces.ProvinceInterface;

public interface IProvinceService
{
    /// <summary>
    /// دریافت تمام استان ها 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<ProvinceResponseDto>> GetAllProvincesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ساخت استان
    /// </summary>
    /// <param name="provinceRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateProvinceAsync(
        CreateProvinceRequestDto provinceRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام استان ها برای دراپ داون 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<ProvinceResponseDto>> GetAllForSelectAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف نرم استان
    /// </summary>
    /// <param name="provinceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SoftDeleteAsync(
        Guid provinceId,
        CancellationToken cancellationToken = default);
}
