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

    #region Get Methods

    public async Task<Pagination<CompanyDetailResponseDto>> GetCityCompaniesAsync(
        Guid cityId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (cityCompanies, totalDataCount) = await _unitOfWork.CompanyCityRepository.GetCityCompaniesAsync(cc => new CompanyDetailResponseDto
        {
            CompanyId = cc.CompanyId,
            CityId = cc.CityId,
            OwnedByUserId = cc.Company.OwnedByUserId,
            CityName = cc.City.Name,
            CompanyName = cc.Company.Name,
            YearOfEstablishment = cc.Company.YearOfEstablishment,
            Industry = cc.Company.Industry,
            AboutUs = cc.Company.AboutUs,
            CompanyImageFileId = cc.Company.CompanyImageFileId
        },
          cityId,
          cancellationToken,
          pagingCommand.PageNumber,
          pagingCommand.PageSize);

        return Pagination<CompanyDetailResponseDto>.GetPagination(cityCompanies,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }

    public async Task<Pagination<CityDetailResponseDto>> GetProvinceCitiesAsync(
        Guid provinceId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (provinceCities, totalDataCount) = await _unitOfWork.CityRepository.GetProvinceCitiesAsync(c => new CityDetailResponseDto
        {
            CityId = c.Id,
            CityName = c.Name,
            CityCode = c.CityCode,
            ProvinceName = c.Province.Name,
            ProvinceId = c.ProvinceId,
            ProvinceCode = c.ProvinceCode
        },
          provinceId,
          cancellationToken,
          pagingCommand.PageNumber,
          pagingCommand.PageSize);

        return Pagination<CityDetailResponseDto>.GetPagination(provinceCities,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }

    #endregion
}
