using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Mvc.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email or phone number is required.")]
    [RegularExpression(
        @"^(?:[^\s@]+@[^\s@]+\.[^\s@]+|(?:\+98|0)9\d{9})$",
        ErrorMessage = "The email or phone number format is invalid.")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}