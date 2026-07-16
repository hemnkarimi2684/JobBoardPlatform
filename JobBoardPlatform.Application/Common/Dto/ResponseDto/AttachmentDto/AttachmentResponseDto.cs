namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;

public class AttachmentResponseDto
{
    /// <summary>
    /// اسم فایل اپلود شده 
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// نوع محتوا فایل اپدیت شده
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// دیتا فایل اپلود شده 
    /// </summary>
    public byte[] Data { get; set; } = [];
}
