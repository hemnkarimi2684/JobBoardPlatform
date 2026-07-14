using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.UserDto.Result;

public record UserProfileInfoResult(
    string FullName,
    string Bio,
    string Address,
    DateTime BirthDate,
    string CityName,
    Gender Gender
    );

