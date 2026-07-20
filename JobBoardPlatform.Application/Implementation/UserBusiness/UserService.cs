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

    public async Task<bool> CreateProfileAsync(CreateProfileRequestDto createCommand)
    {
        var doesUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId);

        if (!doesUserExist)
            throw new NotFoundException($"User with id {createCommand.UserId} was not found.");

        _accessControlService.EnsureApplicantOrAdmin(createCommand.UserId, _currentUser);

        var isDuplicateUserProfile = await _unitOfWork.UserProfileRepository.IsDuplicateUserProfileAsync(createCommand.UserId);

        if (isDuplicateUserProfile)
            throw new ConflictException($"User with id {createCommand.UserId} already has profile");

        var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(createCommand.CityId);

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

        await _unitOfWork.UserProfileRepository.AddAsync(userProfile);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<UserProfileInfoResponseDto> GetUserProfileInfoAsync(Guid userId)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        var userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileInfoAsync(up => new UserProfileInfoResponseDto(
                                                                                    up.UserId,
                                                                                    up.FirstName + " " + up.LastName,
                                                                                    up.Bio,
                                                                                    up.Address,
                                                                                    up.BirthDate,
                                                                                    up.City.Name,
                                                                                    up.Gender
                                                                                    ),
                                                                                      userId);

        if (userProfile is null)
            throw new NotFoundException($"the user profile with id {userId} was not found");

        return userProfile;
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto updateCommand)
    {
        _accessControlService.EnsureApplicantOrAdmin(userId, _currentUser);

        if (updateCommand.CityId != null)
        {
            var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(updateCommand.CityId.Value);

            if (!doesCityExist)
                throw new NotFoundException($"City with id {updateCommand.CityId} was not found.");
        }

        var result = await _unitOfWork.UserProfileRepository.UpdateProfileAsync(userId, MapToUpdateUserProfile(updateCommand));

        if (!result)
            throw new NotFoundException($"the user profile with id {userId} was not found");

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<bool> ApprovedEmployerAsync(Guid userId)
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
            await _unitOfWork.BeginTransactionAsync();

            user.UpdateIsApproved(true, _currentUser.UserId);

            var claim = new Claim(ClaimConstants.EmployerClaimType, ClaimConstants.IsApprovedClaimValue);

            var employerClaims = await _userManager.GetClaimsAsync(user);

            if (!employerClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
            {

                var addClaimResult = await _userManager.AddClaimAsync(user, claim);

                if (!addClaimResult.Succeeded)
                    throw new ValidationException(string.Join(" ", addClaimResult.Errors.Select(e => e.Description)));
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.RollBackTransactionAsync();

            throw;
        }
    }


    #region Private methods

    private UpdateUserProfile MapToUpdateUserProfile(UpdateProfileRequestDto updateCommand)
    {
        return new UpdateUserProfile
        (
          updateCommand.FirstName,
          updateCommand.LastName,
          updateCommand.Bio,
          updateCommand.Address,
          updateCommand.BirthDate,
          updateCommand.CityId,
          updateCommand.Gender,
          _currentUser.UserId
        );
    }

    #endregion
}
