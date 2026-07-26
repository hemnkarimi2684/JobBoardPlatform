using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.JobCategoryDto;

public class CreateJobCategoryRequestDto
{
    [Required(ErrorMessage = "Job category name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Job category name must be between 2 and 100 characters.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Job category name cannot be empty or whitespace.")]
    public string Name { get; set; } = default!;
}
