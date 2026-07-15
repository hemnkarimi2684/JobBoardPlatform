using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Result;

public record AdvertisementDisplayDto(
    string JobTitle,
    string CompanyName,
    string CityName,
    CollaborationType CollaborationType,
    int ExperienceLevel
);
