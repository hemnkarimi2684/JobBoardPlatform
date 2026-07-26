using JobBoardPlatform.Application.Common.Dto.RequestDto.AttachmentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using Microsoft.AspNetCore.Http;

namespace JobBoardPlatform.Application.Interfaces.AttachmentInterface;

public interface IAttachmentService
{
    /// <summary>
    /// اپلود فایل
    /// </summary>
    /// <param name="formFile"></param>
    /// <param name="attachmentType"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> UploadAsync(
        IFormFile formFile,
        AttachmentType attachmentType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دانلود فایل
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadAsync(
        Guid attachmentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// حذف فایل ذخیره شده در دیتابیس 
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> HardDeleteAttachmentAsync(
        Guid attachmentId,
        CancellationToken cancellationToken = default);
}
