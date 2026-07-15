using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public record RegisterRequestDto(
                                [Required(ErrorMessage = "Email is required.")]
                                [ EmailAddress(ErrorMessage = "Email format is invalid.")]
                                string Email,

                                 [Required(ErrorMessage = "Phone number is required.")]
                                 [RegularExpression(
                                     @"^(?:\+98|0)9\d{9}$",
                                     ErrorMessage = "Phone number must start with 09 or +98 and be a valid Iranian mobile number."
                                 )]
                                string PhoneNumber,

                                [Required(ErrorMessage = "Password is required.")]
                                [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
                                [RegularExpression(
                                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
                                    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
                                )]
                                string Password,

                                CreateCompanyRequestDto CreateCompanyRequest);

