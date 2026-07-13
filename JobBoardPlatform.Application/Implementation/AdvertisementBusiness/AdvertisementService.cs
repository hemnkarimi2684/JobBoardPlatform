using JobBoardPlatform.Application.Common.Constants.Authentication;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Command;
using JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Result;
using JobBoardPlatform.Application.Common.Dto.Common.Command;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AdvertisementInterface;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;


namespace JobBoardPlatform.Application.Implementation.AdvertisementBusiness;

public class AdvertisementService : IAdvertisementService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public AdvertisementService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public Task<bool> CreateAdvertisementAsync(CreateAdvertisementCommand createCommand)
    {
        CheckPermissionOfCreate(_currentUser);

    }


    public async Task<Pagination<AdvertisementDetailResult>> GetAdvertisementsByCompanyAsync(PagingCommand pagingCommand, Guid companyId)
    {
        var companyOwnerId = await _unitOfWork.CompanyRepository.GetCompanyOwnerIdByCompanyIdAsync(companyId);

        if (companyOwnerId == null)
            throw new NotFoundException($"The company with id {companyId} was not found.");

        CheckPermissionPerRequest(companyOwnerId, _currentUser);

        var (companyAdvertisements, totalDataCount) = await _unitOfWork.AdvertisementRepository
                                                    .GetAdvertisementsByCompanyAsync(a => new AdvertisementDetailResult
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
                                                        CreatedAt = a.CreatedAt
                                                    },
                                                    companyId,
                                                    pagingCommand.PageNumber,
                                                    pagingCommand.PageSize);

        return Pagination<AdvertisementDetailResult>.GetPagination(
                                                             companyAdvertisements,
                                                             pagingCommand.PageNumber,
                                                             pagingCommand.PageSize,
                                                             totalDataCount);

    }
    public async Task<AdvertisementDetailResult> GetAdvertisementInfoByIdAsync(Guid advertisementId)
    {
        var advertisementDetail = await _unitOfWork.AdvertisementRepository.GetAdvertisementInfoByIdAsync(advertisementId);

        if (advertisementDetail is null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        return AdvertisementDetailResult.MapToResult(advertisementDetail);
    }

    public async Task<bool> SoftDeleteAdvertisementAsync(Guid advertisementId)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        CheckPermissionPerRequest(advertisementOwnerId, _currentUser);

        var advertisementDeleteResult = await _unitOfWork.AdvertisementRepository.SoftDeleteAsync(advertisementId, _currentUser.UserId);

        if (!advertisementDeleteResult)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAdvertisementAsync(Guid advertisementId, UpdateAdvertisementCommand updateCommand)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        CheckPermissionPerRequest(advertisementOwnerId, _currentUser);

        var updateAdvertisementInfoResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementInfoAsync(
                                                                                                   advertisementId,
                                                                                                   MapToAdvertisementInfoUpdate(updateCommand));
        if (!updateAdvertisementInfoResult)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> InActivateAdvertisementAsync(Guid advertisementId)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        CheckPermissionPerRequest(advertisementOwnerId, _currentUser);

        var updateAdvertisementStatusResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementStatusAsync(advertisementId, _currentUser.UserId, false);

        if (!updateAdvertisementStatusResult)
            throw new NotFoundException($"the advertisement with this id {advertisementId} not found ");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> ActivateAdvertisementAsync(Guid advertisementId)
    {
        var advertisementOwnerId = await _unitOfWork.AdvertisementRepository.GetAdvertisementOwnerIdByIdAsync(advertisementId);

        if (advertisementOwnerId == null)
            throw new NotFoundException($"The advertisement with id {advertisementId} was not found.");

        CheckPermissionPerRequest(advertisementOwnerId, _currentUser);

        var updateAdvertisementStatusResult = await _unitOfWork.AdvertisementRepository.UpdateAdvertisementStatusAsync(advertisementId, _currentUser.UserId, true);

        if (!updateAdvertisementStatusResult)
            throw new NotFoundException($"the advertisement with this id {advertisementId} not found ");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }


    #region Private Methods

    private void CheckPermissionPerRequest(Guid? ownerId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new ForbiddenException("User is not available.");

        var isOwner = ownerId == currentUser.UserId;

        var isAdmin = currentUser.UserRoles.Any(role => role == RoleConstants.AdminRoleName);

        var isEmployer = currentUser.UserRoles.Any(role => role == RoleConstants.EmployerRoleName);

        //این شرط برای اینه که اگر ادمینه دسترسی داره اگر ادمین نیس حالا باید چک شه که کارفرماس یا نه
        //حالا اگر کارفرما بود ایا اونره این اگهیه یا نه                                                                  
        if (!isAdmin && !(isOwner && isEmployer))
            throw new ForbiddenException("You do not have sufficient access to update this advertisement.");
    }

    private void CheckPermissionOfCreate(ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new ForbiddenException("User is not available.");

        var isAdminOrEmployer = currentUser.UserRoles.Any(role => role == RoleConstants.EmployerRoleName || role == RoleConstants.AdminRoleName);

        if (!isAdminOrEmployer)
            throw new ForbiddenException("You do not have sufficient access to create a advertisement.");
    }

    private CollaborationType? ParseEnums(string? collaborationType)
    {
        CollaborationType? parsedCollaborationType = null;

        if (!string.IsNullOrWhiteSpace(collaborationType))
        {
            if (!Enum.TryParse<CollaborationType>(collaborationType, true, out var result))
                throw new ValidationException("Invalid CollaborationType type.");

            parsedCollaborationType = result;
        }

        return parsedCollaborationType;
    }

    private UpdateAdvertisementInfo MapToAdvertisementInfoUpdate(UpdateAdvertisementCommand updateAdvertisementCommand)
    {
        var parsedEnum = ParseEnums(updateAdvertisementCommand.CollaborationType);

        return new UpdateAdvertisementInfo
        (
            updateAdvertisementCommand.Description,
            updateAdvertisementCommand.MinimumAge,
            updateAdvertisementCommand.MaximumAge,
            updateAdvertisementCommand.MinimumSalary,
            updateAdvertisementCommand.MaximumSalary,
            updateAdvertisementCommand.ExperienceLevel,
            parsedEnum,
            _currentUser.UserId
        );
    }

    #endregion
}
