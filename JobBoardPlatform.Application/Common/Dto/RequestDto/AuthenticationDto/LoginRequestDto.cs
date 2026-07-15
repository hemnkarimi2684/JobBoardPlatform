using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public record LoginRequestDto(
                              [Required(ErrorMessage = "Email or phone number is required.")]
                              [RegularExpression(
                                  @"^(?:[^\s@]+@[^\s@]+\.[^\s@]+|(?:\+98|0)9\d{9})$",
                                  ErrorMessage = "Email or phone number format is invalid."
                              )]
                              string EmailOrPhoneNumber,

                              [Required(ErrorMessage = "Password is required.")]
                              [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
                              string Password);

