using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public record ResumeExperienceDetailResponseDto(
    Guid ExperienceDetailId,
    string LastJobTitle,
    SeniorityLevel SeniorityLevel,
    string JobCategory,
    string City,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrentJob
);

