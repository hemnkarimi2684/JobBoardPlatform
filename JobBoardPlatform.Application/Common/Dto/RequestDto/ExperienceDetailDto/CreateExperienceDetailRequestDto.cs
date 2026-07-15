using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;

public record CreateExperienceDetailRequestDto(
    string LastJobTitle,
    string SeniorityLevel,
    string JobCategory,
    string City,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsCurrentJob,
    Guid UserId,
    Guid? CreatedById = null
    );

