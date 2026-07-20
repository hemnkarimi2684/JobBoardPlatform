using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public record ResumeUserProfileResponseDto(
    string FullName,
    string Bio,
    string Address,
    DateTime BirthDate,
    string CityName,
    Gender Gender
);
