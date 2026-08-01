namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.EmailTemplateDto;

public class EmailTemplateResponseDto
{
    public Guid Id { get; init; }

    public string Key { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;

    public string Body { get; init; } = string.Empty;

    public bool IsActive { get; init; }
}
