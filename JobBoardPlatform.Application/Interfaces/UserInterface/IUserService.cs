using JobBoardPlatform.Application.Common.Dto.UserDto.Command;
using JobBoardPlatform.Application.Common.Dto.UserDto.Result;
using JobBoardPlatform.Core.Entities.Common.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;

namespace JobBoardPlatform.Application.Interfaces.UserInterface;

public interface IUserService
{
    /// <summary>
    /// ساخت پروفایل برای کاربر 
    /// </summary>
    /// <param name="createCommand"></param>
    /// <returns></returns>
    Task<bool> CreateProfileAsync(CreateProfileCommand createCommand);

    /// <summary>
    /// اپدیت پروفایل کاربر
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileCommand updateCommand);

    /// <summary>
    /// دریافت اطلاعات پروفایل کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserProfileInfoResult> GetUserProfileInfoAsync(Guid userId);
}
