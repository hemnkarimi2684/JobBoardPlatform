using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.Common;
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
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ApprovedEmployerAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// رد کردن کارفرما توسط ادمین
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RejectEmployerAsync(
        Guid userId,
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
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<EmployerDetailResponseDto>> GetApprovedEmployersAsync(PagingRequestDto pagingCommand);

    /// <summary>
    /// دریافت تمام کارفرما های در حال انتظار 
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<EmployerDetailResponseDto>> GetUnapprovedEmployersAsync(PagingRequestDto pagingCommand);

    /// <summary>
    /// دریافت تمام کارجو ها
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <returns></returns>
    Task<Pagination<JobSeekerDetailResponseDto>> GetJobSeekersAsync(PagingRequestDto pagingCommand);

    /// <summary>
    /// فعال کردن اکانت کارجو
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ActivateJobSeekerAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// غیر فعال کردن اکانت کارجو
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeactivateJobSeekerAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات کارفرما و شرکتش
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<EmployerWithCompanyResponseDto> GetEmployerWithCompanyAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام جنسیت ها در سیستم
    /// </summary>
    /// <returns></returns>
    List<EnumResponseDto> GetGenders();

    /// <summary>
    /// حذف عکس پروفایل
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteUserImageAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
