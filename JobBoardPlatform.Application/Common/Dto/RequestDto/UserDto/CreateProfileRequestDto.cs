using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public record CreateProfileRequestDto(
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters.")]
    string FirstName,

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters.")]
    string LastName,

    [StringLength(1000, ErrorMessage = "Bio must not exceed 1000 characters.")]
    string Bio,

    [Required(ErrorMessage = "Address is required.")]
    [StringLength(300, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 300 characters.")]
    string Address,

    [Required(ErrorMessage = "Birth date is required.")]
    DateTime BirthDate,

    [Required(ErrorMessage = "CityId is required.")]
    Guid CityId,

    [Required(ErrorMessage = "UserId is required.")]
    Guid UserId,

    [Required(ErrorMessage = "Gender is required.")]
    Gender Gender
);
