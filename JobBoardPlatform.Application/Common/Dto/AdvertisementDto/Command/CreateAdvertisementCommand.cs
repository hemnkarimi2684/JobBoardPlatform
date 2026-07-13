using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Command;

public record CreateAdvertisementCommand(
    string Description,
    int MinimumAge,
    int MaximumAge, 
    decimal MinimumSalary,
    decimal MaximumSalary, 
    int ExperienceLevel, 
    string CollaborationType, 
    Guid JobId, 
    Guid CityId, 
    Guid CompanyId,
    List<Guid> SkillsId);
