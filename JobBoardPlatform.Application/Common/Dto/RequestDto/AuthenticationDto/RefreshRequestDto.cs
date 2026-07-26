using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public class RefreshRequestDto
{
    [Required(ErrorMessage = "Refresh token is required.")]
    [StringLength(512, MinimumLength = 32, ErrorMessage = "Invalid refresh token format.")]
    public string RefreshToken { get; set; } = string.Empty;
}