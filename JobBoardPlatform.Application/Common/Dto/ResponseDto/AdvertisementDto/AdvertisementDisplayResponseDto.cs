using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;

public class AdvertisementDisplayResponseDto
{
    public string JobTitle { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string CityName { get; init; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CollaborationType CollaborationType { get; init; }

    public int ExperienceLevel { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AdvertisementStatus Status { get; init; }
}
