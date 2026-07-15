using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Implementation.CityBusiness;

public class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public CityService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Pagination<CompanyDetailResponseDto>> GetCityCompaniesAsync(Guid cityId, PagingRequestDto pagingCommand)
    {
        var (cityCompanies, totalDataCount) = await _unitOfWork.CompanyCityRepository.GetCityCompaniesAsync(cc => new CompanyDetailResponseDto
                                                                                         (
                                                                                           cc.City.Name,
                                                                                           cc.Company.Name,
                                                                                           cc.Company.YearOfEstablishment,
                                                                                           cc.Company.Industry,
                                                                                           cc.Company.AboutUs
                                                                                         ),
                                                                                         cityId,
                                                                                         pagingCommand.PageNumber,
                                                                                         pagingCommand.PageSize);

        return Pagination<CompanyDetailResponseDto>.GetPagination(cityCompanies,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }

    public async Task<Pagination<CityDetailResponseDto>> GetProvinceCitiesAsync(Guid provinceId, PagingRequestDto pagingCommand)
    {
        var (provinceCities, totalDataCount) = await _unitOfWork.CityRepository.GetProvinceCitiesAsync(c => new CityDetailResponseDto
                                                                                         (
                                                                                           c.Name,
                                                                                           c.CityCode,
                                                                                           c.Province.Name,
                                                                                           c.ProvinceCode
                                                                                         ),
                                                                                         provinceId,
                                                                                         pagingCommand.PageNumber,
                                                                                         pagingCommand.PageSize);

        return Pagination<CityDetailResponseDto>.GetPagination(provinceCities,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }
}
