using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;


namespace JobBoardPlatform.Application.Implementation.AdvertisementBusiness;

public class AdvertisementService : IAdvertisementService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    public AdvertisementService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
    }

    #region Create Methods

    public async Task<bool> CreateAdvertisementAsync(
        CreateAdvertisementRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        await ValidateForCreateAsync(createCommand.JobId, createCommand.CompanyId, createCommand.CityId, cancellationToken);

        var advertisement = new Advertisement(createCommand.Description,
                                              createCommand.MinimumAge,
                                              createCommand.MaximumAge,
                                              createCommand.MinimumSalary,
                                              createCommand.MaximumSalary,
                                              createCommand.ExperienceLevel,
                                              createCommand.CollaborationType,
                                              createCommand.JobId,
                                              createCommand.CityId,
                                              createCommand.CompanyId,
                                              _currentUser.UserId);

        await _unitOfWork.AdvertisementRepository.AddAsync(advertisement, cancellationToken);

        if (createCommand.SkillsId is not null && createCommand.SkillsId.Any())
        {
            foreach (var skillId in createCommand.SkillsId.Distinct())
            {
                var advertisementSkill = new AdvertisementSkill(advertisement.Id, skillId, _currentUser.UserId);

                await _unitOfWork.AdvertisementSkillRepository.AddAsync(advertisementSkill, cancellationToken);
            }
        }

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<AdvertisementDetailResponseDto>> GetAdvertisementsByCompanyAsync(
        PagingRequestDto pagingCommand,
        Guid companyId,
        CancellationToken cancellationToken = default)
    {
        var companyOwnerId = await _unitOfWork.CompanyRepository.GetCompanyOwnerIdByCompanyIdAsync(companyId, cancellationToken);

        if (companyOwnerId == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(companyOwnerId.Value, _currentUser);

        var (companyAdvertisements, totalDataCount) = await _unitOfWork.AdvertisementRepository
                                                    .GetAdvertisementsByCompanyAsync(a => new AdvertisementDetailResponseDto
                                                    {
                                                        Description = a.Description,
                                                        MinimumAge = a.MinimumAge,
                                                        MaximumAge = a.MaximumAge,
                                                        MinimumSalary = a.MinimumSalary,
                                                        MaximumSalary = a.MaximumSalary,
                                                        ExperienceLevel = a.ExperienceLevel,
                                                        CollaborationType = a.CollaborationType,
                                                        CityName = a.City.Name,
                                                        CompanyName = a.Company.Name,
                                                        JobName = a.Job.Name,
                                                        AboutCompany = a.Company.AboutUs,
                                                        Industry = a.Company.Industry,
                                                        CreatedAt = a.CreatedAt,
                                                        AdvertisementId = a.Id,
                                                        CityId = a.CityId,
                                                        CompanyId = a.CompanyId,
                                                        SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
                                                    },
                                                    companyId,
                                                    cancellationToken,
                                                    pagingCommand.PageNumber,
                                                    pagingCommand.PageSize);

        return Pagination<AdvertisementDetailResponseDto>.GetPagination(
                                                             companyAdvertisements,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);

    }

    public async Task<AdvertisementDisplayResponseDto> GetAdvertisementProjectionAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        var result = await _unitOfWork.AdvertisementRepository.GetAdvertisementProjectionAsync(a => new AdvertisementDisplayResponseDto
        {
            CityName = a.City.Name,
            CollaborationType = a.CollaborationType,
            CompanyName = a.Company.Name,
            ExperienceLevel = a.ExperienceLevel,
            JobTitle = a.Job.Name
        }, advertisementId, cancellationToken);

        if (result == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        return result;
    }

    public async Task<AdvertisementDetailResponseDto> GetAdvertisementInfoByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        var advertisementDetail = await _unitOfWork.AdvertisementRepository.GetAdvertisementInfoByIdAsync(advertisementId, cancellationToken);

        if (advertisementDetail is null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        return AdvertisementDetailResponseDto.MapToResponseDto(advertisementDetail);
    }

    public async Task<Pagination<AdvertisementDetailResponseDto>> GetActiveAdvertisementsAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.AdvertisementRepository.QueryAsync(a => new AdvertisementDetailResponseDto
        {
            Description = a.Description,
            MinimumAge = a.MinimumAge,
            MaximumAge = a.MaximumAge,
            MinimumSalary = a.MinimumSalary,
            MaximumSalary = a.MaximumSalary,
            ExperienceLevel = a.ExperienceLevel,
            CollaborationType = a.CollaborationType,
            CityName = a.City.Name,
            CompanyName = a.Company.Name,
            JobName = a.Job.Name,
            AboutCompany = a.Company.AboutUs,
            Industry = a.Company.Industry,
            CreatedAt = a.CreatedAt,
            AdvertisementId = a.Id,
            CityId = a.CityId,
            CompanyId = a.CompanyId,
            SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
        },
        a => a.IsActive,
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);
    }


    #endregion

    #region Delete Methods

    public async Task<bool> SoftDeleteAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId, cancellationToken);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(advertisementOwnerId.Value, _currentUser);

        var advertisementDeleteResult = await _unitOfWork.AdvertisementRepository.SoftDeleteAsync(advertisementId, _currentUser.UserId, cancellationToken);

        if (!advertisementDeleteResult)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        var updateAdvertisementStatusResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementStatusAsync(
            advertisementId,
            _currentUser.UserId,
            false,
            cancellationToken);

        if (!updateAdvertisementStatusResult)
            throw new NotFoundException($"the advertisement with this id {advertisementId} not found ");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Update Methods

    public async Task<bool> UpdateAdvertisementAsync(
        Guid advertisementId,
        UpdateAdvertisementRequestDto updateCommand,
        CancellationToken cancellationToken = default)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId, cancellationToken);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(advertisementOwnerId.Value, _currentUser);

        var updateAdvertisementInfoResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementInfoAsync(
                                                                                                   advertisementId,
                                                                                                   cancellationToken,
                                                                                                   MapToAdvertisementInfoUpdate(updateCommand));
        if (!updateAdvertisementInfoResult)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> InActivateAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId, cancellationToken);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(advertisementOwnerId.Value, _currentUser);

        var updateAdvertisementStatusResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementStatusAsync(
            advertisementId,
            _currentUser.UserId,
            false,
            cancellationToken);

        if (!updateAdvertisementStatusResult)
            throw new NotFoundException($"the advertisement with this id {advertisementId} not found ");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> ActivateAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId, cancellationToken);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(advertisementOwnerId.Value, _currentUser);

        var updateAdvertisementStatusResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementStatusAsync(
            advertisementId,
            _currentUser.UserId,
            true,
            cancellationToken);

        if (!updateAdvertisementStatusResult)
            throw new NotFoundException($"the advertisement with this id {advertisementId} not found ");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Private Methods

    private UpdateAdvertisementInfo MapToAdvertisementInfoUpdate(UpdateAdvertisementRequestDto updateAdvertisementCommand)
    {
        return new UpdateAdvertisementInfo
        {
            Description = updateAdvertisementCommand.Description,
            MinimumAge = updateAdvertisementCommand.MinimumAge < 1 ? null : updateAdvertisementCommand.MinimumAge,
            MaximumAge = updateAdvertisementCommand.MaximumAge < 1 ? null : updateAdvertisementCommand.MaximumAge,
            MinimumSalary = updateAdvertisementCommand.MinimumSalary < 1 ? null : updateAdvertisementCommand.MinimumSalary,
            MaximumSalary = updateAdvertisementCommand.MaximumSalary < 1 ? null : updateAdvertisementCommand.MaximumSalary,
            ExperienceLevel = updateAdvertisementCommand.ExperienceLevel < 1 ? null : updateAdvertisementCommand.ExperienceLevel,
            CollaborationType = updateAdvertisementCommand.CollaborationType,
            ModifiedById = _currentUser.UserId
        };
    }

    private async Task ValidateForCreateAsync(Guid jobId, Guid companyId, Guid cityId, CancellationToken cancellationToken)
    {
        var companyOwnerId = await _unitOfWork.CompanyRepository.GetCompanyOwnerIdByCompanyIdAsync(companyId, cancellationToken);

        if (companyOwnerId == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(companyOwnerId.Value, _currentUser);

        var isJobExist = await _unitOfWork.JobRepository.IsJobExistAsync(jobId, cancellationToken);

        if (!isJobExist)
            throw new NotFoundException($"the job with id {jobId} was not found");

        var isCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(cityId, cancellationToken);

        if (!isCityExist)
            throw new NotFoundException($"the city with id {cityId} was not found");
    }

    #endregion
}
