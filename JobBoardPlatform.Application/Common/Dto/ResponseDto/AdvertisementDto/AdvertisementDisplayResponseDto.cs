using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;

public record AdvertisementDisplayResponseDto(
    string JobTitle,
    string CompanyName,
    string CityName,
    CollaborationType CollaborationType,
    int ExperienceLevel
);
