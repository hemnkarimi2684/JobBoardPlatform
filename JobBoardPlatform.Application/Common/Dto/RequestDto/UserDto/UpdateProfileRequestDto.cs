using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public record UpdateProfileRequestDto(
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters.")]
    string? FirstName,

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters.")]
    string? LastName,

    [StringLength(1000, ErrorMessage = "Bio must not exceed 1000 characters.")]
    string? Bio,

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 300 characters.")]
    string? Address,

    DateTime? BirthDate,

    Guid? CityId,

    Gender? Gender
);

