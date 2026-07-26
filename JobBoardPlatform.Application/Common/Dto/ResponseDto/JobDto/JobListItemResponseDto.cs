namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;

public class JobListItemResponseDto
{
    public Guid JobId { get; init; }

    public string Name { get; init; } = string.Empty;
}
