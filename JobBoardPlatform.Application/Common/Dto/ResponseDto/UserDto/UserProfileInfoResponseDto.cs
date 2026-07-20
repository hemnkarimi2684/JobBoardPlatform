using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

public record UserProfileInfoResponseDto(
    string FullName,
    string Bio,
    string Address,
    DateTime BirthDate,
    string CityName,
    Gender Gender
    );

