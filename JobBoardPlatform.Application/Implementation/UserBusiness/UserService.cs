using JobBoardPlatform.Application.Common.AccessClaims.UserClaim;
using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.Constants.RoleConstant;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.AttachmentInterface;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.UserEntity.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace JobBoardPlatform.Application.Implementation.UserBusiness;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    private readonly IAttachmentService _attachmentService;

    private readonly IUserDapperRepository _userDapperRepository;

    private readonly IEmailService _emailService;

    private readonly UserManager<User> _userManager;

    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService, IAttachmentService attachmentService, IUserDapperRepository userDapperRepository, IEmailService emailService, UserManager<User> userManager, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
        _attachmentService = attachmentService;
        _userDapperRepository = userDapperRepository;
        _emailService = emailService;
        _userManager = userManager;
        _logger = logger;
    }

    #region Create Methods

    public async Task<bool> CreateProfileAsync(
        CreateProfileRequestDto createCommand,
        CancellationToken cancellationToken = default)
    {
        var doesUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId, cancellationToken);

        if (!doesUserExist)
            throw new NotFoundException($"User with id {createCommand.UserId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(createCommand.UserId, _currentUser);

        var isDuplicateUserProfile = await _unitOfWork.UserProfileRepository.IsDuplicateUserProfileAsync(createCommand.UserId, cancellationToken);

        if (isDuplicateUserProfile)
            throw new ConflictException($"User with id {createCommand.UserId} already has profile");

        var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(createCommand.CityId, cancellationToken);

        if (!doesCityExist)
            throw new NotFoundException($"City with id {createCommand.CityId} was not found.");

        var userProfile = new UserProfile(
                                          createCommand.FirstName,
                                          createCommand.LastName,
                                          createCommand.Bio,
                                          createCommand.Address,
                                          createCommand.BirthDate,
                                          createCommand.UserId,
                                          createCommand.CityId,
                                          createCommand.Gender,
                                          null,
                                          _currentUser.UserId
                                          );

        await _unitOfWork.UserProfileRepository.AddAsync(userProfile, cancellationToken);

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    #endregion

    #region Get methods

    public async Task<UserProfileResponseDto> GetUserProfileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileByUserIdAsync(up => new UserProfileResponseDto
        {
            UserId = up.UserId,
            FullName = up.FirstName + " " + up.LastName,
            Bio = up.Bio,
            Address = up.Address,
            BirthDate = up.BirthDate,
            CityName = up.City.Name,
            Gender = up.Gender,
            UserImageFileId = up.UserImageFileId
        },
          userId, cancellationToken);

        if (userProfile is null)
            throw new NotFoundException($"the user profile with id {userId} was not found");

        return userProfile;
    }

    public async Task<EmployerWithCompanyResponseDto> GetEmployerWithCompanyAsync(
         Guid ownerId,
         CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var result = await _unitOfWork.CompanyRepository.GetCompanyByOwnerIdAsync(c => new EmployerWithCompanyResponseDto
        {
            CompanyId = c.Id,
            Name = c.Name,
            Email = c.OwnedByUser.Email!,
            PhoneNumber = c.OwnedByUser.PhoneNumber!,
            UserId = c.OwnedByUserId,
            YearOfEstablishment = c.YearOfEstablishment,
            JobCategoryId = c.JobCategoryId,
            JobCategoryName = c.JobCategory.Name,
            AboutUs = c.AboutUs,
            WebSiteAddress = c.WebSiteAddress,
            OwnershipType = c.OwnershipType,
            CompanySize = c.CompanySize,
            ActivityType = c.ActivityType,
            CompanyImageFileId = c.CompanyImageFileId,
            Cities = c.CompanyCities.Select(cc => cc.CityId).ToList()
        },
          ownerId,
          cancellationToken);

        if (result is null)
            throw new NotFoundException($"the company with this ownerId {ownerId} not found");

        return result;
    }

    public async Task<Pagination<EmployerDetailResponseDto>> GetApprovedEmployersAsync(
         PagingRequestDto pagingCommand)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var (result, totalDataCount) = await _userDapperRepository.GetApprovedEmployersAsync(pagingCommand.PageNumber, pagingCommand.PageSize);

        return Pagination<EmployerDetailResponseDto>.GetPagination(
                                                        EmployerDetailResponseDto.MapToResponseDto(result),
                                                        pagingCommand.PageNumber,
                                                        pagingCommand.PageSize,
                                                        totalDataCount);
    }

    public async Task<Pagination<JobSeekerDetailResponseDto>> GetJobSeekersAsync(
         PagingRequestDto pagingCommand)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var (result, totalDataCount) = await _userDapperRepository.GetJobSeekersAsync(pagingCommand.PageNumber, pagingCommand.PageSize);

        return Pagination<JobSeekerDetailResponseDto>.GetPagination(
                                                        JobSeekerDetailResponseDto.MapToResponseDto(result),
                                                        pagingCommand.PageNumber,
                                                        pagingCommand.PageSize,
                                                        totalDataCount);
    }

    #endregion

    #region Update Methods

    public async Task<bool> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequestDto updateCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        if (updateCommand.CityId != null)
        {
            var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(updateCommand.CityId.Value, cancellationToken);

            if (!doesCityExist)
                throw new NotFoundException($"City with id {updateCommand.CityId} was not found.");
        }

        var result = await _unitOfWork.UserProfileRepository.UpdateProfileAsync(
                                                                                userId,
                                                                                cancellationToken,
                                                                                MapToUpdateUserProfile(updateCommand));

        if (!result)
            throw new NotFoundException($"the user profile with id {userId} was not found");

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task ApprovedEmployerAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new NotFoundException($"user with id {userId} was not found.");

        var isEmployer = await _userManager.IsInRoleAsync(user, RoleConstants.EmployerRoleName);

        if (!isEmployer)
            throw new ValidationException($"the user with id {user.Id} is not an employer");

        if (user.IsApproved)
            throw new ConflictException($"the employer with id {user.Id} is already approved");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            user.UpdateIsApproved(true, _currentUser.UserId);

            var claim = new Claim(UserClaims.EmployerClaimType, UserClaims.IsApprovedClaimValue);

            var employerClaims = await _userManager.GetClaimsAsync(user);

            if (!employerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {

                var result = await _userManager.AddClaimAsync(user, claim);

                if (!result.Succeeded)
                    throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }

        await _emailService.SendAsync(user.Email!, "Approved Employer Account", "Your account has been verified.", false, cancellationToken);
    }

    public async Task RejectEmployerAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new NotFoundException($"user with id {userId} was not found.");

        var isEmployer = await _userManager.IsInRoleAsync(user, RoleConstants.EmployerRoleName);

        if (!isEmployer)
            throw new ValidationException($"the user with id {user.Id} is not an employer");

        if (!user.IsApproved)
            throw new ConflictException($"the employer with id {user.Id} is already not approved");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            user.UpdateIsApproved(false, _currentUser.UserId);

            var claim = new Claim(UserClaims.EmployerClaimType, UserClaims.IsApprovedClaimValue);

            var employerClaims = await _userManager.GetClaimsAsync(user);

            if (employerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                var result = await _userManager.RemoveClaimAsync(user, claim);

                if (!result.Succeeded)
                    throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }

        await _emailService.SendAsync(user.Email!, "Reject Employer Account", "Your account was rejected.", false, cancellationToken);
    }

    public async Task ActivateJobSeekerAsync(
    Guid userId,
    CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new NotFoundException($"user with id {userId} was not found.");

        var isJobSeeker = await _userManager.IsInRoleAsync(user, RoleConstants.JobSeekerRoleName);

        if (!isJobSeeker)
            throw new ValidationException($"the user with id {user.Id} is not a jobSeeker");

        if (user.IsActive)
            throw new ConflictException($"the jobSeeker with id {user.Id} is already active");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            user.UpdateIsActive(true, _currentUser.UserId);

            var claim = new Claim(UserClaims.JobSeekerClaimType, UserClaims.IsActiveClaimValue);

            var jobSeekerClaims = await _userManager.GetClaimsAsync(user);

            if (!jobSeekerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                var result = await _userManager.AddClaimAsync(user, claim);

                if (!result.Succeeded)
                    throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }
    }

    public async Task DeactivateJobSeekerAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new NotFoundException($"user with id {userId} was not found.");

        var isJobSeeker = await _userManager.IsInRoleAsync(user, RoleConstants.JobSeekerRoleName);

        if (!isJobSeeker)
            throw new ValidationException($"the user with id {user.Id} is not a jobSeeker");

        if (!user.IsActive)
            throw new ConflictException($"The JobSeeker with id {user.Id} is already inactive.");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            user.UpdateIsActive(false, _currentUser.UserId);

            var claim = new Claim(UserClaims.JobSeekerClaimType, UserClaims.IsActiveClaimValue);

            var jobSeekerClaims = await _userManager.GetClaimsAsync(user);

            if (jobSeekerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {
                var result = await _userManager.RemoveClaimAsync(user, claim);

                if (!result.Succeeded)
                    throw new ValidationException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }
    }

    #endregion

    #region Upload User Image 

    public async Task UploadUserImageAsync(
        Guid userId,
        UploadUserImageRequestDto imageRequestDto,
        CancellationToken cancellationToken = default)
    {
        if (imageRequestDto?.Image is null)
            throw new ValidationException("image is required.");

        var isUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(userId, cancellationToken);

        if (!isUserExist)
            throw new NotFoundException($"the user with id {userId} was not found");

        var userProfile = await _unitOfWork.UserProfileRepository.GetProfileByUserIdAsync(userId, cancellationToken);

        if (userProfile is null)
            throw new NotFoundException($"The user with id '{userId}' does not have a profile.");

        _accessControlService.EnsureApplicant(userProfile.UserId, _currentUser);

        await UploadImageAsync(userProfile, imageRequestDto.Image, cancellationToken);
    }

    #endregion

    #region DownLoad User Image

    public async Task<AttachmentResponseDto> DownloadUserImageAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {

        var userProfile = await _unitOfWork.UserProfileRepository.GetProfileByUserIdAsync(userId, cancellationToken);

        if (userProfile is null)
            throw new NotFoundException($"The user with id '{userId}' does not have a profile.");

        if (userProfile.UserImageFileId is null)
            throw new NotFoundException($"The user with id '{userId}' does not have an attached image.");

        return await _attachmentService.DownloadAsync(userProfile.UserImageFileId.Value, cancellationToken);
    }


    #endregion

    #region Private methods

    private UpdateUserProfile MapToUpdateUserProfile(UpdateProfileRequestDto updateCommand)
    {
        return new UpdateUserProfile
        {
            FirstName = updateCommand.FirstName,
            LastName = updateCommand.LastName,
            Bio = updateCommand.Bio,
            Address = updateCommand.Address,
            BirthDate = updateCommand.BirthDate,
            CityId = updateCommand.CityId,
            Gender = updateCommand.Gender,
            ModifiedById = _currentUser.UserId
        };
    }

    private async Task DeleteAttachmentAsync(Guid attachmentId, CancellationToken cancellationToken)
    {
        try
        {
            await _attachmentService.HardDeleteAttachmentAsync(attachmentId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete attachment {AttachmentId}", attachmentId);
        }
    }

    private async Task UploadImageAsync(UserProfile userProfile, IFormFile image, CancellationToken cancellationToken)
    {
        //نگه داشتن ایدی قبلی عکس اپلود شده 
        var oldImageId = userProfile.UserImageFileId;
        Guid? newImageId = null;

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            newImageId = await _attachmentService.UploadAsync(image, AttachmentType.Image, cancellationToken);

            userProfile.UpdateImage(newImageId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            //اینجا برای این ترای کچ کذاشتم که اگه توی فلو اضافه کردن و اپدیت کردن عکس به رزومه به اکسپشن و مشکلی خورد....
            //وعکس جدیدی اپلود شده بود اما بدون اینکه به کاربر اختصاص داشته باشه اینو بیام حذف کنم 
            if (newImageId != null)
                await DeleteAttachmentAsync(newImageId.Value, cancellationToken);

            throw;
        }

        //حالا اگه عکس جدیدی سیو شد و اپدیت شد بیا اون عکس قدیمی رو حذف کن 
        if (oldImageId != null)
            await DeleteAttachmentAsync(oldImageId.Value, cancellationToken);
    }

    #endregion
}
