namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;

public class AttachmentResponseDto
{
    public Guid AttachmentId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public byte[] Data { get; set; } = [];
}
