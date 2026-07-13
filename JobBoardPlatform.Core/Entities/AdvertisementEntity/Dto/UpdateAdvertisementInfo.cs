using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;

public record UpdateAdvertisementInfo(
string? Description,
int? MinimumAge,
int? MaximumAge,
decimal? MinimumSalary,
decimal? MaximumSalary,
int? ExperienceLevel,
CollaborationType? CollaborationType,
Guid? ModifiedById);
