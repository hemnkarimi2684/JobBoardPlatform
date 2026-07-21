using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;

public class CreateExperienceDetailRequestDto
{
    [Required(ErrorMessage = "Last job title is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last job title must be between 2 and 100 characters.")]
    public string LastJobTitle { get; set; } = string.Empty;

    [EnumDataType(typeof(SeniorityLevel), ErrorMessage = "Invalid seniority level.")]
    public SeniorityLevel SeniorityLevel { get; set; }

    [Required(ErrorMessage = "Job category is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job category must be between 2 and 100 characters.")]
    public string JobCategory { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
    public string City { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsCurrentJob { get; set; }
}
