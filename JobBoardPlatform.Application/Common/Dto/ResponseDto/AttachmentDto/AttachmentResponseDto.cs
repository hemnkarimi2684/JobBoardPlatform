namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AttachmentDto;

public class AttachmentResponseDto
{
    public Guid AttachmentId { get; init; }

    public string FileName { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public byte[] Data { get; init; } = [];
}
