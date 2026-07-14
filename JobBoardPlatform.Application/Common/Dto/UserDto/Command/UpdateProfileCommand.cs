using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.UserDto.Command;

public record UpdateProfileCommand(
    string? FirstName,
    string? LastName,
    string? Bio,
    string? Address,
    DateTime? BirthDate,
    Guid? CityId,
    string? Gender
);

