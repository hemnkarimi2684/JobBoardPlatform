using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.CityDto.Result;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Dto.CompanyDto.Result;
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

    public async Task<Pagination<CompanyDetailResult>> GetCityCompaniesAsync(Guid cityId, PagingCommand pagingCommand)
    {
        var (cityCompanies, totalDataCount) = await _unitOfWork.CompanyCityRepository.GetCityCompaniesAsync(cc => new CompanyDetailResult
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

        return Pagination<CompanyDetailResult>.GetPagination(cityCompanies,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }

    public async Task<Pagination<CityDetailResult>> GetProvinceCitiesAsync(Guid provinceId, PagingCommand pagingCommand)
    {
        var (provinceCities, totalDataCount) = await _unitOfWork.CityRepository.GetProvinceCitiesAsync(c => new CityDetailResult
                                                                                         (
                                                                                           c.Name,
                                                                                           c.CityCode,
                                                                                           c.Province.Name,
                                                                                           c.ProvinceCode
                                                                                         ),
                                                                                         provinceId,
                                                                                         pagingCommand.PageNumber,
                                                                                         pagingCommand.PageSize);

        return Pagination<CityDetailResult>.GetPagination(provinceCities,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }
}
