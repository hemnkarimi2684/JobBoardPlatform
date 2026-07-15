using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.UserDto;

public record CreateProfileRequestDto(
    string FirstName,
    string LastName,
    string Bio,
    string Address,
    DateTime BirthDate,
    Guid CityId,
    Guid UserId,
    string Gender
);

