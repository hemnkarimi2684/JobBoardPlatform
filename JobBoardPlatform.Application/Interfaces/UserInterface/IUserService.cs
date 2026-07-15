using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
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
    Task<bool> CreateProfileAsync(CreateProfileRequestDto createCommand);

    /// <summary>
    /// اپدیت پروفایل کاربر
    /// </summary>
    /// <param name="updateCommand"></param>
    /// <returns></returns>
    Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto updateCommand);

    /// <summary>
    /// دریافت اطلاعات پروفایل کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<UserProfileInfoResponseDto> GetUserProfileInfoAsync(Guid userId);

    /// <summary>
    /// تایید کردن کارفرما توسط ادمین
    /// </summary>
    /// <param name="employerId"></param>
    /// <returns></returns>
    Task<bool> ApprovedEmployerAsync(Guid employerId);
}
