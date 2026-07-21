using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public class ResumeExperienceDetailResponseDto
{
    public Guid ExperienceDetailId { get; init; }

    public string LastJobTitle { get; init; } = string.Empty;

    public SeniorityLevel SeniorityLevel { get; init; }

    public string JobCategory { get; init; } = string.Empty;

    public string City { get; init; } = string.Empty;

    public DateTime StartDate { get; init; }

    public DateTime? EndDate { get; init; }

    public bool IsCurrentJob { get; init; }
}

