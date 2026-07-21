using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;

public class JobApplicationInfoResponseDto
{
    public Guid JobApplicationId { get; init; }

    public string JobTitle { get; init; } = string.Empty;

    public string CompanyName { get; init; } = string.Empty;

    public string CityName { get; init; } = string.Empty;

    public CollaborationType CollaborationType { get; init; }

    public int ExperienceLevel { get; init; }

    public JobApplicationStatus Status { get; init; }

    public DateTime CreatedAt { get; init; }

    public string UserProfileName { get; init; } = string.Empty;

    public Guid ResumeId { get; init; }

    public Guid AdvertisementId { get; init; }

    public Guid UserId { get; init; }
}