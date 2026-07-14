using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ExperienceDetailDto.Command;

public record UpdateExperienceDetailCommand(
    string? LastJobTitle,
    string? SeniorityLevel,
    string? JobCategory,
    string? City,
    DateTime? StartDate,
    DateTime? EndDate,
    bool? IsCurrentJob,
    Guid? ModifiedById = null);

