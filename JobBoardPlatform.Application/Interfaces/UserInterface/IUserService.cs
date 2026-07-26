using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
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
    Task<UserProfileResponseDto> GetUserProfileByUserIdAsync(
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

    /// <summary>
    /// رد کردن کارفرما توسط ادمین
    /// </summary>
    /// <param name="employerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> InApprovedEmployerAsync(
        Guid employerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دانلود عکس پروفایل کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadUserImageAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپلود عکس پروفایل کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="imageRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UploadUserImageAsync(
        Guid userId,
        UploadUserImageRequestDto imageRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام کارفرما های فعال در سیستم 
    /// </summary>
    /// <returns></returns>
    Task<EmployerResponseDto> GetApprovedEmployersAsync(
        PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام کارجو ها
    /// </summary>
    /// <param name="pagingRequestDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<JobSeekerResponseDto> GetJobSeekersAsync(
        PagingRequestDto pagingRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// فعال یا غیر فعال کردن اکانت کارجو
    /// </summary>
    /// <param name="jobSeekerId"></param>
    /// <param name="isActive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> SetJobSeekerActivationAsync(
        Guid jobSeekerId,
        bool isActive,
        CancellationToken cancellationToken = default);
}
