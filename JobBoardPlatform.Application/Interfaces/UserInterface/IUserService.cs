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
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CreateProfileAsync(
        CreateProfileRequestDto createCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپدیت پروفایل کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="updateCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequestDto updateCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات پروفایل کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserProfileInfoResponseDto> GetUserProfileInfoAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// تایید کردن کارفرما توسط ادمین
    /// </summary>
    /// <param name="employerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ApprovedEmployerAsync(
        Guid employerId,
        CancellationToken cancellationToken = default);
}
