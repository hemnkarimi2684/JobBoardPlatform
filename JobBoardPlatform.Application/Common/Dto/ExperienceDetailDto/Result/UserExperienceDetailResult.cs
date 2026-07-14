using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ExperienceDetailDto.Result;

public record UserExperienceDetailResult(
    string LastJobTitle,
    SeniorityLevel SeniorityLevel,
    string JobCategory,
    string City,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrentJob
);
