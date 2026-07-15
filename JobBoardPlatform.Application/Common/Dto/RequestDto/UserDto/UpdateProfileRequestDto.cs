using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public record UpdateProfileRequestDto(
    string? FirstName,
    string? LastName,
    string? Bio,
    string? Address,
    DateTime? BirthDate,
    Guid? CityId,
    string? Gender
);

