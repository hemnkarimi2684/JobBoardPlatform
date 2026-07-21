using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public class ResumeUserProfileResponseDto
{
    public string FullName { get; init; } = string.Empty;

    public string Bio { get; init; } = string.Empty;

    public string Address { get; init; } = string.Empty;

    public DateTime BirthDate { get; init; }

    public string CityName { get; init; } = string.Empty;

    public Gender Gender { get; init; }
}