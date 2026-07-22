using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;

namespace JobBoardPlatform.Application.Implementation.CityBusiness;

public class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public CityService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Create Methods

    public async Task CreateCityAsync(
        CreateCityRequestDto cityRequestDto,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var provinceCode = await _unitOfWork.ProvinceRepository.GetProvinceCodeAsync(cityRequestDto.ProvinceId, cancellationToken);

        if (provinceCode == 0)
            throw new NotFoundException("");

        var isDuplicateNameOrCode = await _unitOfWork.CityRepository
                                                .IsDuplicateNameOrCodeAsync(cityRequestDto.Name, cityRequestDto.Code, cancellationToken);

        if (isDuplicateNameOrCode)
            throw new ConflictException("");

        var city = new City(cityRequestDto.Name, cityRequestDto.Code, provinceCode, cityRequestDto.ProvinceId, _currentUser.UserId);

        await _unitOfWork.CityRepository.AddAsync(city, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<CompanyListItemResponseDto>> GetCityCompaniesAsync(
        Guid cityId,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (cityCompanies, totalDataCount) = await _unitOfWork.CompanyCityRepository.GetCityCompaniesAsync(cc => new CompanyListItemResponseDto
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

        return Pagination<CompanyListItemResponseDto>.GetPagination(cityCompanies,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);
    }

    public async Task<Pagination<CityDetailResponseDto>> GetAllCitiesAsync(PagingRequestDto pagingCommand, CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CityRepository.QueryAsync(c => new CityDetailResponseDto
        {
            CityId = c.Id,
            CityName = c.Name,
            CityCode = c.CityCode,
            ProvinceName = c.Province.Name,
            ProvinceId = c.ProvinceId,
            ProvinceCode = c.ProvinceCode
        },
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);
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

    public async Task<CityDetailResponseDto> GetCityByIdAsync(
        Guid cityId,
        CancellationToken cancellationToken = default)
    {
        var city = await _unitOfWork.CityRepository.GetByIdAsync(cityId, cancellationToken);

        if (city == null)
            throw new NotFoundException($"teh city with id {cityId} was not found.");

        return new CityDetailResponseDto
        {
            CityId = city.Id,
            CityName = city.Name,
            CityCode = city.CityCode,
            ProvinceName = city.Province.Name,
            ProvinceId = city.ProvinceId,
            ProvinceCode = city.ProvinceCode
        };
    }

    #endregion
}
