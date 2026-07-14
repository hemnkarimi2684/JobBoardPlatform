using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.UserDto.Command;

public record CreateProfileCommand(
    string FirstName,
    string LastName,
    string Bio,
    string Address,
    DateTime BirthDate,
    Guid CityId,
    Guid UserId,
    string Gender
);

