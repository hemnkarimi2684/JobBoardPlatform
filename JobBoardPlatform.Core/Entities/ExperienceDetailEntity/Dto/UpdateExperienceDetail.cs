using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;

public record UpdateExperienceDetail(
    string? LastJobTitle,
    SeniorityLevel? SeniorityLevel,
    string? JobCategory,
    string? City,
    DateTime? StartDate,
    DateTime? EndDate,
    bool? IsCurrentJob,
    Guid? ModifiedById = null);

