using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.AdvertisementDto.Result;

public class AdvertisementDetailResult
{
    public string Description { get; set; } = string.Empty;

    public int MinimumAge { get; set; }

    public int MaximumAge { get; set; }

    public decimal MinimumSalary { get; set; }

    public decimal MaximumSalary { get; set; }

    public int ExperienceLevel { get; set; }

    public DateTime CreatedAt { get; set; }

    public CollaborationType CollaborationType { get; set; }

    public string CityName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string JobName { get; set; } = string.Empty;

    public string AboutCompany { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public List<string> SkillNames { get; set; } = new();

    public static AdvertisementDetailResult MapToResult(AdvertisementDetail advertisementDetail)
    {
        return new AdvertisementDetailResult
        {
            Description = advertisementDetail.Description,
            AboutCompany = advertisementDetail.AboutCompany,
            CreatedAt = advertisementDetail.CreatedAt,
            MaximumAge = advertisementDetail.MaximumAge,
            MinimumAge = advertisementDetail.MinimumAge,
            CityName = advertisementDetail.CityName,
            CollaborationType = advertisementDetail.CollaborationType,
            MaximumSalary = advertisementDetail.MaximumSalary,
            MinimumSalary = advertisementDetail.MinimumSalary,
            CompanyName = advertisementDetail.CompanyName,
            ExperienceLevel = advertisementDetail.ExperienceLevel,
            Industry = advertisementDetail.Industry,
            JobName = advertisementDetail.JobName,
            SkillNames = advertisementDetail.SkillNames,
        };
    }
}
