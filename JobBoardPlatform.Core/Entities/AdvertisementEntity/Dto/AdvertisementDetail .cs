using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;

public record AdvertisementDetail(
string Description,
int MinimumAge,
int MaximumAge,
decimal MinimumSalary,
decimal MaximumSalary,
int ExperienceLevel,
CollaborationType CollaborationType,
string CityName,
string CompanyName,
string JobName,
string AboutCompany,
string Industry,
DateTime CreatedAt,
List<string> SkillNames,
Guid AdvertisementId,
Guid CityId,
Guid CompanyId
);

