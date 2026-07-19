using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;

public record CreateExperienceDetailRequestDto(
    [Required(ErrorMessage = "Last job title is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last job title must be between 2 and 100 characters.")]
    string LastJobTitle,

    [Required(ErrorMessage = "Seniority level is required.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Seniority level must be between 2 and 50 characters.")]
    string SeniorityLevel,

    [Required(ErrorMessage = "Job category is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job category must be between 2 and 100 characters.")]
    string JobCategory,

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
    string City,

    DateTime StartDate,

    DateTime? EndDate,

    bool IsCurrentJob,

    Guid UserId
);
