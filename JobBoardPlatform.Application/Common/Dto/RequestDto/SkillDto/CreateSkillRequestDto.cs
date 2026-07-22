using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.SkillDto;

public class CreateSkillRequestDto
{
    [Required(ErrorMessage = "Skill name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Skill name must be between 2 and 100 characters.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Skill name cannot be empty or whitespace.")]
    public string Name { get; set; } = default!;
}