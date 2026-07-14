using JobBoardPlatform.Application.Common.Constants.Authentication;
using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Command;
using JobBoardPlatform.Application.Common.Dto.UserDto.Command;
using JobBoardPlatform.Application.Common.Dto.UserDto.Result;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.UserInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Implementation.UserBusiness;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    public UserService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<bool> CreateProfileAsync(CreateProfileCommand createCommand)
    {
        var doesUserExist = await _unitOfWork.UserRepository.IsUserExistAsync(createCommand.UserId);

        if (!doesUserExist)
            throw new NotFoundException($"User with id {createCommand.UserId} was not found.");

        CheckSelfOrAdminPermission(createCommand.UserId, _currentUser);

        var isDuplicateUserProfile = await _unitOfWork.UserProfileRepository.IsDuplicateUserProfileAsync(createCommand.UserId);

        if (isDuplicateUserProfile)
            throw new ConflictException($"User with id {createCommand.UserId} already has profile");

        var doesCityExist = await _unitOfWork.CityRepository.IsCityExistAsync(createCommand.CityId);

        if (!doesCityExist)
            throw new NotFoundException($"City with id {createCommand.CityId} was not found.");

        var gender = ParseGenderForCreate(createCommand.Gender);

        var userProfile = new UserProfile(
                                          createCommand.FirstName,
                                          createCommand.LastName,
                                          createCommand.Bio,
                                          createCommand.Address,
                                          createCommand.BirthDate,
                                          createCommand.UserId,
                                          createCommand.CityId,
                                          gender,
                                          null,
                                          _currentUser.UserId
                                          );

        await _unitOfWork.UserProfileRepository.AddAsync(userProfile);

        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async Task<UserProfileInfoResult> GetUserProfileInfoAsync(Guid userId)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

        var userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileInfoAsync(up => new UserProfileInfoResult(
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

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileCommand updateCommand)
    {
        CheckSelfOrAdminPermission(userId, _currentUser);

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

    #region Private methods

    private void CheckSelfOrAdminPermission(Guid targetUserId, ICurrentUser currentUser)
    {
        if (currentUser.UserId == null)
            throw new UnauthorizedException("User is not authenticated.");

        var isSelfUser = targetUserId == currentUser.UserId.Value;

        var isAdmin = currentUser.UserRoles.Any(role => role == RoleConstants.AdminRoleName);

        //اینجا چک میشه که کاربر فقط بتونه خودش اطلاعات مدرک تحصیلیش رو اپدیت کنه نه کس دیگه ای به جز ادمین                                                               
        if (!isAdmin && !isSelfUser)
            throw new ForbiddenException("You do not have sufficient access to manage this advertisement.");
    }

    private Gender ParseGenderForCreate(string gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
            throw new ValidationException("gender is required.");

        if (!Enum.TryParse<Gender>(gender, true, out var result))
            throw new ValidationException("Invalid gender type.");

        return result;
    }

    private Gender? ParseGenderEnumForUpdate(string? gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
            return null;

        if (!Enum.TryParse<Gender>(gender, true, out var result))
            throw new ValidationException("Invalid gender type.");

        return result;
    }

    private UpdateUserProfile MapToUpdateUserProfile(UpdateProfileCommand updateCommand)
    {
        var gender = ParseGenderEnumForUpdate(updateCommand.Gender);

        return new UpdateUserProfile
        (
          updateCommand.FirstName,
          updateCommand.LastName,
          updateCommand.Bio,
          updateCommand.Address,
          updateCommand.BirthDate,
          updateCommand.CityId,
          gender,
          _currentUser.UserId
        );
    }

    #endregion
}
