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
    /// <returns></returns>
    Task<bool> CreateResumeAsync(CreateResumeRequestDto resumeCommand);

    /// <summary>
    /// اپلود فایل رزومه توسط شناسه رزومه
    /// </summary>
    /// <param name="resumeId"></param>
    /// <param name="uploadResumeFile"></param>
    /// <returns></returns>
    Task UploadResumeFileByResumeIdAsync(Guid resumeId, UploadResumeFileRequestDto uploadResumeFile);

    /// <summary>
    /// دریافت فایل رزومه توسط شناسه رزومه 
    /// </summary>
    /// <param name="resumeId"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadResumeFileByResumeIdAsync(Guid resumeId);

    /// <summary>
    /// اپلود فایل رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="uploadResumeFile"></param>
    /// <returns></returns>
    Task UploadResumeFileByUserIdAsync(Guid userId, UploadResumeFileRequestDto uploadResumeFile);

    /// <summary>
    /// دریافت فایل رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadResumeFileByUserIdAsync(Guid userId);

    /// <summary>
    /// حذف فایل رزومه 
    /// </summary>
    /// <param name="resumeId"></param>
    /// <returns></returns>
    Task<bool> DeleteResumeFileByIdAsync(Guid resumeId);

    /// <summary>
    /// دریافت اطلاعات رزومه توسط شناسه کاربر 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<ResumeDetailResponseDto> GetResumeDetailAsync(Guid userId);
}
