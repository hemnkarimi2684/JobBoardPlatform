using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Email or phone number is required.")]
    [RegularExpression(
        @"^(?:[^\s@]+@[^\s@]+\.[^\s@]+|(?:\+98|0)9\d{9})$",
        ErrorMessage = "Email or phone number format is invalid.")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8,
        ErrorMessage = "Password must be between 8 and 100 characters long.")]
    public string Password { get; set; } = string.Empty;
}

