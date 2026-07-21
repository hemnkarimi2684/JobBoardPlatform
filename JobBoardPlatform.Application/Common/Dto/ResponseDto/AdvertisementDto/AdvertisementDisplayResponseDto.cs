using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;

public class AdvertisementDisplayResponseDto
{
    public string JobTitle { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string CityName { get; init; } = string.Empty;

    public CollaborationType CollaborationType { get; init; }

    public int ExperienceLevel { get; init; }
}
