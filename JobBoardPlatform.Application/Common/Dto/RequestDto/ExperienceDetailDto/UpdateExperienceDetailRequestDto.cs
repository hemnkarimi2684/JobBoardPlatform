using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;

public record UpdateExperienceDetailRequestDto(
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last job title must be between 2 and 100 characters.")]
    string? LastJobTitle,

    [StringLength(50, MinimumLength = 2, ErrorMessage = "Seniority level must be between 2 and 50 characters.")]
    string? SeniorityLevel,

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job category must be between 2 and 100 characters.")]
    string? JobCategory,

    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
    string? City,

    DateTime? StartDate,

    DateTime? EndDate,

    bool? IsCurrentJob
);

