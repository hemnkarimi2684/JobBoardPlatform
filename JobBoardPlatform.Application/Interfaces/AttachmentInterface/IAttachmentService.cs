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
    /// <param name="uploadFileRequest"></param>
    /// <returns></returns>
    Task<Guid> UploadAsync(IFormFile formFile, AttachmentType attachmentType);

    /// <summary>
    /// دانلود فایل
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <returns></returns>
    Task<AttachmentResponseDto> DownloadAsync(Guid attachmentId);

    /// <summary>
    /// حذف فایل ذخیره شده در دیتابیس 
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <returns></returns>
    Task<bool> HardDeleteAttachmentAsync(Guid attachmentId);
}
