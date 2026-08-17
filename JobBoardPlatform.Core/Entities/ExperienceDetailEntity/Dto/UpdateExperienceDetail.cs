using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;

public class UpdateExperienceDetail
{
    public SeniorityLevel? SeniorityLevel { get; init; }

    public string? LastJobTitle { get; init; }
    public string? JobCategory { get; init; }
    public string? City { get; init; }

    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }

    public bool? IsCurrentJob { get; init; }

    public Guid? ModifiedById { get; init; }
}
