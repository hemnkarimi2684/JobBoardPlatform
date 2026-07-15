using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;

public record UserExperienceDetailResponseDto(
    string LastJobTitle,
    SeniorityLevel SeniorityLevel,
    string JobCategory,
    string City,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrentJob
);
