using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;

public class UpdateExperienceDetailRequestDto
{
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last job title must be between 2 and 100 characters.")]
    public string? LastJobTitle { get; set; }

    public SeniorityLevel? SeniorityLevel { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job category must be between 2 and 100 characters.")]
    public string? JobCategory { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "City must be between 2 and 100 characters.")]
    public string? City { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool? IsCurrentJob { get; set; }
}
