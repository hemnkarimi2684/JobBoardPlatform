using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.Common;

public class TextRequestDto
{
    [MaxLength(100, ErrorMessage = "text term cannot be longer than 100 characters.")]
    public string? Text { get; set; } = string.Empty;
}
