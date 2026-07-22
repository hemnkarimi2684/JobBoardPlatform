using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AdvertisementDto;

public class AdvertisementDetailResponseDto
{
    public Guid AdvertisementId { get; set; }

    public Guid JobId { get; set; }

    public Guid CityId { get; set; }

    public Guid CompanyId { get; set; }

    public string Description { get; set; } = string.Empty;

    public int MinimumAge { get; set; }

    public int MaximumAge { get; set; }

    public decimal MinimumSalary { get; set; }

    public decimal MaximumSalary { get; set; }

    public int ExperienceLevel { get; set; }

    public DateTime CreatedAt { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CollaborationType CollaborationType { get; set; }

    public string CityName { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public string JobName { get; set; } = string.Empty;

    public string AboutCompany { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public List<string> SkillNames { get; set; } = new();

    public static AdvertisementDetailResponseDto MapToResponseDto(AdvertisementDetail advertisementDetail)
    {
        return new AdvertisementDetailResponseDto
        {
            Description = advertisementDetail.Description,
            JobId = advertisementDetail.JobId,
            AboutCompany = advertisementDetail.CompanyAboutUs,
            CreatedAt = advertisementDetail.CreatedAt,
            MaximumAge = advertisementDetail.MaximumAge,
            MinimumAge = advertisementDetail.MinimumAge,
            CityName = advertisementDetail.CityName,
            CollaborationType = advertisementDetail.CollaborationType,
            MaximumSalary = advertisementDetail.MaximumSalary,
            MinimumSalary = advertisementDetail.MinimumSalary,
            CompanyName = advertisementDetail.CompanyName,
            ExperienceLevel = advertisementDetail.ExperienceLevel,
            Industry = advertisementDetail.CompanyIndustry,
            JobName = advertisementDetail.JobName,
            SkillNames = advertisementDetail.Skills,
            AdvertisementId = advertisementDetail.AdvertisementId,
            CityId = advertisementDetail.CityId,
            CompanyId = advertisementDetail.CompanyId,
        };
    }
}
