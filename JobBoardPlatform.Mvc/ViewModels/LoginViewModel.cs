using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Mvc.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "ایمیل یا شماره موبایل الزامی است.")]
    [RegularExpression(
        @"^(?:[^\s@]+@[^\s@]+\.[^\s@]+|(?:\+98|0)9\d{9})$",
        ErrorMessage = "قالب ایمیل یا شماره موبایل معتبر نیست.")]
    public string EmailOrPhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "رمز عبور الزامی است.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
