using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;

public record UpdateUserProfile(
    string? FirstName,
    string? LastName,
    string? Bio,
    string? Address,
    DateTime? BirthDate,
    Guid? CityId,
    Gender? Gender,
    Guid? ModifiedById
);

