using JobBoardPlatform.Application.Common.Constants;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JobBoardPlatform.Application.Implementation.UserBusiness;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    private readonly UserManager<User> _userManager;

    public UserService(IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
        _userManager = userManager;
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
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

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

    public async Task<bool> ApprovedEmployerAsync(
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

        if (user.IsApproved == true)
            throw new ConflictException($"the employer with id {user.Id} is already approved");

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            user.UpdateIsApproved(true, _currentUser.UserId);

            var claim = new Claim(ClaimConstants.EmployerClaimType, ClaimConstants.IsApprovedClaimValue);

            var employerClaims = await _userManager.GetClaimsAsync(user);

            if (!employerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {

                var addClaimResult = await _userManager.AddClaimAsync(user, claim);

                if (!addClaimResult.Succeeded)
                    throw new ValidationException(string.Join(" ", addClaimResult.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync(cancellationToken);

            throw;
        }
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

    #endregion
}
