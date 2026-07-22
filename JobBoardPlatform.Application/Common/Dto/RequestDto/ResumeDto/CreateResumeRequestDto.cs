using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.ResumeDto.Command;

public class CreateResumeRequestDto
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid UserId { get; set; }
}

