using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CityDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.RedisKeys;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.CityInterface;
using JobBoardPlatform.Application.Interfaces.RedisInterface;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Implementation.CityBusiness;

public class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    private readonly IRedisService _redisService;

    public CityService(
        IUnitOfWork unitOfWork, 
        ICurrentUser currentUser,
        IAccessControlService accessControlService,
        IRedisService redisService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
        _redisService = redisService;
    }

    #region Create Methods

    public async Task CreateCityAsync(
        CreateCityRequestDto cityRequestDto,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var provinceCode = await _unitOfWork.ProvinceRepository.GetProvinceCodeAsync(cityRequestDto.ProvinceId, cancellationToken);

        if (provinceCode == 0)
            throw new NotFoundException("The specified province was not found.");

        var isDuplicateNameOrCode = await _unitOfWork.CityRepository
                                                .IsDuplicateNameOrCodeAsync(cityRequestDto.Name, cityRequestDto.Code, cancellationToken);

        if (isDuplicateNameOrCode)
            throw new ConflictException("A city with the same name or code already exists.");

        var city = new City(cityRequestDto.Name, cityRequestDto.Code, provinceCode, cityRequestDto.ProvinceId, _currentUser.UserId);

        await _unitOfWork.CityRepository.AddAsync(city, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _redisService.RemoveAsync(RedisCacheKeys.CitiesSelect);
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
            JobCategoryId = cc.Company.JobCategoryId,
            JobCategoryName = cc.Company.JobCategory.Name,
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

    public async Task<Pagination<CityDetailResponseDto>> GetAllCitiesAsync(
        TextRequestDto textRequestDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.CityRepository.GetAllCitiesAsync(c => new CityDetailResponseDto
        {
            CityId = c.Id,
            CityName = c.Name,
            CityCode = c.CityCode,
            ProvinceName = c.Province.Name,
            ProvinceId = c.ProvinceId,
            ProvinceCode = c.ProvinceCode
        },
        textRequestDto.Text,
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);

        return Pagination<CityDetailResponseDto>.GetPagination(result, pagingCommand.PageNumber, pagingCommand.PageSize, totalDataCount);
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
        var city = await _unitOfWork.CityRepository.GetCityByIdAsync(c => new CityDetailResponseDto
        {
            CityId = c.Id,
            CityName = c.Name,
            CityCode = c.CityCode,
            ProvinceName = c.Province.Name,
            ProvinceId = c.ProvinceId,
            ProvinceCode = c.ProvinceCode
        },
        cityId, cancellationToken);

        if (city == null)
            throw new NotFoundException("city was not found.");

        return city;
    }

    public async Task<List<CityDetailResponseDto>> GetAllForSelectAsync(CancellationToken cancellationToken = default)
    {
        var cached = await _redisService.GetAsync<List<CityDetailResponseDto>>(RedisCacheKeys.CitiesSelect);

        if (cached is not null)
            return cached;

        var result = await _unitOfWork.CityRepository.GetAllForSelectAsync(c => new CityDetailResponseDto
        {
            CityCode = c.CityCode,
            CityId = c.Id,
            CityName = c.Name,
            ProvinceCode = c.ProvinceCode,
            ProvinceId = c.ProvinceId,
            ProvinceName = c.Province.Name
        }, cancellationToken);

        await _redisService.SetAsync(RedisCacheKeys.CitiesSelect, result);

        return result;
    }

    #endregion

    #region Delete Methods

    public async Task SoftDeleteAsync(Guid cityId, CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var result = await _unitOfWork.CityRepository.SoftDeleteAsync(cityId, _currentUser.UserId, cancellationToken);

        if (!result)
            throw new ValidationException($"Could not delete city");

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _redisService.RemoveAsync(RedisCacheKeys.CitiesSelect);
    }

    #endregion
}
