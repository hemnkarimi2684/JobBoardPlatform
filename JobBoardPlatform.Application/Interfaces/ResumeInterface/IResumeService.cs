using JobBoardPlatform.Application.Common.Dto.RequestDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;
using JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using System.Linq.Expressions;

namespace JobBoardPlatform.Application.Interfaces.ResumeInterface;

public interface IResumeService
{
    /// <summary>
    /// ساخت رزومه برای کاربر 
    /// </summary>
    /// <param name="resumeCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> CreateResumeAsync(
        CreateResumeRequestDto resumeCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپلود فایل رزومه توسط شناسه رزومه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="uploadResumeFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UploadResumeFileByResumeIdAsync(
        Guid resumeId,
        UploadResumeFileRequestDto uploadResumeFile,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت فایل رزومه توسط شناسه رزومه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadResumeFileByResumeIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپلود فایل رزومه توسط شناسه کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="uploadResumeFile"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UploadResumeFileByUserIdAsync(
        Guid userId,
        UploadResumeFileRequestDto uploadResumeFile,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت فایل رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadResumeFileByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف فایل رزومه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> DeleteResumeFileByIdAsync(
        Guid resumeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت اطلاعات رزومه توسط شناسه کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResumeDetailResponseDto> GetResumeDetailAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
