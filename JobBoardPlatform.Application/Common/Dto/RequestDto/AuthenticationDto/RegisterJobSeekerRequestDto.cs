using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public class RegisterJobSeekerRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(
        @"^(?:\+98|0)9\d{9}$",
        ErrorMessage = "Phone number format is invalid. Use a valid Iranian mobile number (e.g., 0912...)."
    )]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
        ErrorMessage = "Password must include uppercase, lowercase, number, and special character."
    )]
    public string Password { get; set; } = string.Empty;
}
