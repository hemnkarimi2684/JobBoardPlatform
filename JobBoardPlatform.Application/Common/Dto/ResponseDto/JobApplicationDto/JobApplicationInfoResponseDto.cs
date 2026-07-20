using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;

public record JobApplicationInfoResponseDto(
    string JobTitle,
    string CompanyName,
    string CityName,
    CollaborationType CollaborationType,
    int ExperienceLevel,
    JobApplicationStatus Status,
    DateTime CreatedAt,
    string UserProfileName
    );