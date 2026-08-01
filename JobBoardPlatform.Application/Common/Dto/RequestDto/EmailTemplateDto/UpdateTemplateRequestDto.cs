using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.EmailTemplateDto;

public class UpdateTemplateRequestDto
{
    [Required(ErrorMessage = "Subject is required.")]
    [MinLength(3, ErrorMessage = "Subject must be at least 3 characters long.")]
    [MaxLength(255, ErrorMessage = "Subject cannot exceed 255 characters.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Body is required.")]
    [MinLength(10, ErrorMessage = "Body must be at least 10 characters long.")]
    public string Body { get; set; } = string.Empty;
}