using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AttachmentDto;

public class UploadFileRequestDto
{
    public IFormFile File { get; set; } = default!;

    [EnumDataType(typeof(AttachmentType))]
    public AttachmentType AttachmentType { get; set; }
}
