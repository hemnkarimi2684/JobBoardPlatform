using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Common.Helper;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using System.Linq.Expressions;


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
            throw new NotFoundException("Company was not found.");

        _accessControlService.EnsureOwnerEmployer(companyOwnerId.Value, _currentUser);

        var (companyAdvertisements, totalDataCount) = await _unitOfWork.AdvertisementRepository
                                                    .GetAdvertisementsByCompanyAsync(a => new AdvertisementDetailResponseDto
                                                    {
                                                        Description = a.Description,
                                                        JobId = a.JobId,
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
                                                        CompanyJobCategoryId = a.Company.JobCategoryId,
                                                        CompanyJobCategoryName = a.Company.JobCategory.Name,
                                                        CreatedAt = a.CreatedAt,
                                                        AdvertisementId = a.Id,
                                                        CityId = a.CityId,
                                                        CompanyId = a.CompanyId,
                                                        FeaturedUntil = a.FeaturedUntil,
                                                        IsFeatured = a.IsFeatured,
                                                        IsActive = a.IsActive,
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
            throw new NotFoundException("Advertisement was not found.");

        return result;
    }

    public async Task<AdvertisementDetailResponseDto> GetAdvertisementInfoByIdAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        var advertisementDetail = await _unitOfWork.AdvertisementRepository.GetAdvertisementInfoByIdAsync(advertisementId, cancellationToken);

        if (advertisementDetail is null)
            throw new NotFoundException("Advertisement was not found.");

        return AdvertisementDetailResponseDto.MapToResponseDto(advertisementDetail, _currentUser.UserId);
    }

    public async Task<Pagination<AdvertisementDetailResponseDto>> GetActiveAdvertisementsAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.AdvertisementRepository.QueryAsync(a => new AdvertisementDetailResponseDto
        {
            Description = a.Description,
            JobId = a.JobId,
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
            CompanyJobCategoryId = a.Company.JobCategoryId,
            CompanyJobCategoryName = a.Company.JobCategory.Name,
            CreatedAt = a.CreatedAt,
            AdvertisementId = a.Id,
            CityId = a.CityId,
            CompanyId = a.CompanyId,
            FeaturedUntil = a.FeaturedUntil,
            IsFeatured = a.IsFeatured,
            IsActive = a.IsActive,
            SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
        },
        a => a.IsActive,
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);
    }

    public async Task<Pagination<AdvertisementDetailResponseDto>> GetAllAdvertisementsAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        return await _unitOfWork.AdvertisementRepository.QueryAsync(a => new AdvertisementDetailResponseDto
        {
            Description = a.Description,
            JobId = a.JobId,
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
            CompanyJobCategoryId = a.Company.JobCategoryId,
            CompanyJobCategoryName = a.Company.JobCategory.Name,
            CreatedAt = a.CreatedAt,
            AdvertisementId = a.Id,
            CityId = a.CityId,
            CompanyId = a.CompanyId,
            FeaturedUntil = a.FeaturedUntil,
            IsFeatured = a.IsFeatured,
            IsActive = a.IsActive,
            SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
        },
        cancellationToken,
        pagingCommand.PageNumber,
        pagingCommand.PageSize);
    }

    public async Task<Pagination<AdvertisementDetailResponseDto>> SearchAdvertisementsAsync(
        AdvertisementSearchRequestDto searchDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchDto.SearchTerm))
            return new Pagination<AdvertisementDetailResponseDto>()
            {
                Data = new List<AdvertisementDetailResponseDto>(),
                PageNumber = pagingCommand.PageNumber,
                PageSize = pagingCommand.PageSize,
                TotalPageCount = 0
            }; ;

        var (result, totalDataCount) = await _unitOfWork.AdvertisementRepository.SearchAdvertisementsAsync(
            searchDto.SearchTerm,
            a => new AdvertisementDetailResponseDto
            {
                Description = a.Description,
                JobId = a.JobId,
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
                CompanyJobCategoryId = a.Company.JobCategoryId,
                CompanyJobCategoryName = a.Company.JobCategory.Name,
                CreatedAt = a.CreatedAt,
                AdvertisementId = a.Id,
                CityId = a.CityId,
                CompanyId = a.CompanyId,
                FeaturedUntil = a.FeaturedUntil,
                IsFeatured = a.IsFeatured,
                IsActive = a.IsActive,
                SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
            },
              cancellationToken, pagingCommand.PageNumber, pagingCommand.PageSize);

        return Pagination<AdvertisementDetailResponseDto>.GetPagination(
                                                            result,
                                                            pagingCommand.PageNumber,
                                                            pagingCommand.PageSize,
                                                            totalDataCount);
    }

    public async Task<Pagination<AdvertisementDetailResponseDto>> SearchAndFilterAdvertisementsAsync(
        AdvertisementSearchRequestDto searchDto,
        AdvertisementFilterRequestDto filterDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var predicate = BuildSearchFilterPredicate(searchDto, filterDto);

        return await _unitOfWork.AdvertisementRepository.QueryAsync(
            a => new AdvertisementDetailResponseDto
            {
                Description = a.Description,
                JobId = a.JobId,
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
                CompanyJobCategoryId = a.Company.JobCategoryId,
                CompanyJobCategoryName = a.Company.JobCategory.Name,
                CreatedAt = a.CreatedAt,
                AdvertisementId = a.Id,
                CityId = a.CityId,
                CompanyId = a.CompanyId,
                FeaturedUntil = a.FeaturedUntil,
                IsFeatured = a.IsFeatured,
                IsActive = a.IsActive,
                SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
            },
            predicate,
            cancellationToken,
            pagingCommand.PageNumber,
            pagingCommand.PageSize);
    }

    private static Expression<Func<Advertisement, bool>> BuildSearchFilterPredicate(
        AdvertisementSearchRequestDto searchDto,
        AdvertisementFilterRequestDto filterDto)
    {
        var parameter = Expression.Parameter(typeof(Advertisement), "a");

        Expression? combined = null;

        if (!string.IsNullOrWhiteSpace(searchDto.SearchTerm))
        {
            var term = searchDto.SearchTerm.Trim();
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            var termConstant = Expression.Constant(term);

            var jobLike = Expression.Call(
                Expression.Property(Expression.Property(parameter, nameof(Advertisement.Job)), nameof(Advertisement.Job.Name)),
                containsMethod,
                termConstant);

            var cityLike = Expression.Call(
                Expression.Property(Expression.Property(parameter, nameof(Advertisement.City)), nameof(Advertisement.City.Name)),
                containsMethod,
                termConstant);

            var companyLike = Expression.Call(
                Expression.Property(Expression.Property(parameter, nameof(Advertisement.Company)), nameof(Advertisement.Company.Name)),
                containsMethod,
                termConstant);

            combined = Expression.OrElse(jobLike, Expression.OrElse(cityLike, companyLike));
        }

        if (filterDto.JobCategoryId.HasValue)
        {
            var jobCategoryId = Expression.Property(
                Expression.Property(parameter, nameof(Advertisement.Job)),
                nameof(Advertisement.Job.JobCategoryId));

            var equal = Expression.Equal(jobCategoryId, Expression.Constant(filterDto.JobCategoryId.Value));

            combined = combined is null ? equal : Expression.AndAlso(combined, equal);
        }

        if (filterDto.CollaborationType.HasValue)
        {
            var collaborationType = Expression.Property(parameter, nameof(Advertisement.CollaborationType));

            var equal = Expression.Equal(collaborationType, Expression.Constant(filterDto.CollaborationType.Value));

            combined = combined is null ? equal : Expression.AndAlso(combined, equal);
        }

        if (filterDto.MinimumSalary.HasValue)
        {
            var minimumSalary = Expression.Property(parameter, nameof(Advertisement.MinimumSalary));

            var greaterOrEqual = Expression.GreaterThanOrEqual(minimumSalary, Expression.Constant(filterDto.MinimumSalary.Value));

            combined = combined is null ? greaterOrEqual : Expression.AndAlso(combined, greaterOrEqual);
        }

        if (filterDto.MaximumSalary.HasValue)
        {
            var maximumSalary = Expression.Property(parameter, nameof(Advertisement.MaximumSalary));

            var lessOrEqual = Expression.LessThanOrEqual(maximumSalary, Expression.Constant(filterDto.MaximumSalary.Value));

            combined = combined is null ? lessOrEqual : Expression.AndAlso(combined, lessOrEqual);
        }

        if (combined is null)
            combined = Expression.Constant(true);

        return Expression.Lambda<Func<Advertisement, bool>>(combined, parameter);
    }

    public async Task<Pagination<AdvertisementDetailResponseDto>> FilterAdvertisementsAsync(
        AdvertisementFilterRequestDto filterDto,
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        var (result, totalDataCount) = await _unitOfWork.AdvertisementRepository.FilterAdvertisementsAsync(
            filterDto.MapToQueryFilter(),
            a => new AdvertisementDetailResponseDto
            {
                Description = a.Description,
                JobId = a.JobId,
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
                CompanyJobCategoryId = a.Company.JobCategoryId,
                CompanyJobCategoryName = a.Company.JobCategory.Name,
                CreatedAt = a.CreatedAt,
                AdvertisementId = a.Id,
                CityId = a.CityId,
                CompanyId = a.CompanyId,
                FeaturedUntil = a.FeaturedUntil,
                IsFeatured = a.IsFeatured,
                IsActive = a.IsActive,
                SkillNames = a.AdvertisementSkills.Select(s => s.Skill.Name).ToList()
            },
              cancellationToken, pagingCommand.PageNumber, pagingCommand.PageSize);

        return Pagination<AdvertisementDetailResponseDto>.GetPagination(
                                                            result,
                                                            pagingCommand.PageNumber,
                                                            pagingCommand.PageSize,
                                                            totalDataCount);
    }

    public List<EnumResponseDto> GetCollaborationTypes()
    {
        var collaborationTypes = EnumHelper.GetEnumValues<CollaborationType>();

        if (collaborationTypes is null)
            throw new NotFoundException("No collaboration types are currently defined in the system.");

        return collaborationTypes;
    }

    #endregion

    #region Delete Methods

    public async Task<bool> SoftDeleteAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var advertisementDeleteResult = await _unitOfWork.AdvertisementRepository.SoftDeleteAsync(advertisementId, _currentUser.UserId, cancellationToken);

        if (!advertisementDeleteResult)
            throw new NotFoundException("Advertisement was not found.");

        var updateAdvertisementStatusResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementStatusAsync(
            advertisementId,
            _currentUser.UserId,
            false,
            cancellationToken);

        if (!updateAdvertisementStatusResult)
            throw new NotFoundException("Could not update status as the advertisement was not found.");

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
            throw new NotFoundException("Advertisement was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(advertisementOwnerId.Value, _currentUser);

        var updateAdvertisementInfoResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementInfoAsync(
                                                                                                   advertisementId,
                                                                                                   cancellationToken,
                                                                                                   MapToAdvertisementInfoUpdate(updateCommand));
        if (!updateAdvertisementInfoResult)
            throw new NotFoundException("Advertisement was not found.");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> DeactivateAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var advertisement = await _unitOfWork.AdvertisementRepository.GetByIdAsync(advertisementId, cancellationToken, true);

        if (advertisement is null)
            throw new NotFoundException("Advertisement was not found.");

        if (!advertisement.IsActive)
            throw new ValidationException("The advertisement is already inactive.");

        advertisement.UpdateActiveStatus(_currentUser.UserId, false);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> ActivateAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var advertisement = await _unitOfWork.AdvertisementRepository.GetByIdAsync(advertisementId, cancellationToken, true);

        if (advertisement is null)
            throw new NotFoundException("Advertisement was not found.");

        if (advertisement.IsActive)
            throw new ValidationException("The advertisement is already active.");

        advertisement.UpdateActiveStatus(_currentUser.UserId, true);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task PromoteAdvertisementAsync(
        Guid advertisementId,
        int durationInDays,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        if (durationInDays != 7 && durationInDays != 15 && durationInDays != 30)
            throw new ValidationException("Allowed durations are 7 or 15 or 30 days.");

        var advertisement = await _unitOfWork.AdvertisementRepository.GetByIdAsync(advertisementId, cancellationToken, true);

        if (advertisement == null)
            throw new NotFoundException("Advertisement was not found.");

        //بررسی میکنم اینجا که  این ایا اگهی هنوز فعال است یا نه 
        if (advertisement.IsFeatured && advertisement.FeaturedUntil.HasValue && advertisement.FeaturedUntil.Value >= DateTime.UtcNow)
            throw new ValidationException("the advertisement is already Featured");

        //اینجا چون خود پراپرتی فیچر انتیلم نال است برای پر کردنش از تاریخ دقیق روز استفاده میکنم چرا مطمئنم چون بالا چک کردم که اگه
        // بزرگتر یا مساوی از تاریخ امروز باشه یعنی هنوز فعاله
        var featuredUntil = DateTime.UtcNow.AddDays(durationInDays);

        advertisement.UpdateFeatured(true, featuredUntil);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DemoteAdvertisementAsync(
        Guid advertisementId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var advertisement = await _unitOfWork.AdvertisementRepository.GetByIdAsync(advertisementId, cancellationToken, true);

        if (advertisement == null)
            throw new NotFoundException("Advertisement was not found.");

        //با این شرط اینجا گذاشتم اون اگهی هایی که تاریخ انقضاشون گشذشته هم کامل منقضی میکنم 
        if (advertisement.FeaturedUntil == null && advertisement.IsFeatured == false)
            throw new ValidationException("The advertisement is already in normal (not featured) status.");

        advertisement.UpdateFeatured(false, null);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
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
            throw new NotFoundException("Company was not found.");

        _accessControlService.EnsureOwnerEmployerOrAdmin(companyOwnerId.Value, _currentUser);

        var isJobExist = await _unitOfWork.JobRepository.IsJobExistAsync(jobId, cancellationToken);

        if (!isJobExist)
            throw new NotFoundException("Job category was not found.");

        var isCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(cityId, cancellationToken);

        if (!isCityExist)
            throw new NotFoundException("City was not found.");

        var isCompanyExistInCity = await _unitOfWork.CompanyCityRepository.IsCompanyExistInCityAsync(companyId, cityId, cancellationToken);

        if (!isCompanyExistInCity)
            throw new NotFoundException("The selected company does not operate in the specified city.");
    }

    #endregion
}
