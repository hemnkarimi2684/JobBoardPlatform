using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.JobDto;

public class CreateJobRequestDto
{
    [Required(ErrorMessage = "Job name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job name must be between 2 and 100 characters.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Job name cannot be empty or whitespace.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid JobCategoryId { get; set; }
}
