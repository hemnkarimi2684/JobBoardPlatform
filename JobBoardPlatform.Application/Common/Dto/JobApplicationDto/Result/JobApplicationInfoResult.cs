using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.JobApplicationDto.Result;

public record JobApplicationInfoResult(
    string JobTitle,
    string CompanyName,
    string CityName,
    CollaborationType CollaborationType,
    int ExperienceLevel,
    JobApplicationStatus Status,
    DateTime CreatedAt,
    string UserProfileName
    );