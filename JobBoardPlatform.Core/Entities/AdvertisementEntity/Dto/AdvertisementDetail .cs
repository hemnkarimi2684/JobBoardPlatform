using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;

public class AdvertisementDetail
{
    public Guid AdvertisementId { get; init; }
    public Guid JobId { get; init; }
    public string Description { get; init; } = string.Empty;
    public int MinimumAge { get; init; }
    public int MaximumAge { get; init; }
    public decimal MinimumSalary { get; init; }
    public decimal MaximumSalary { get; init; }
    public int ExperienceLevel { get; init; }
    public CollaborationType CollaborationType { get; init; }
    public string CityName { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string JobName { get; init; } = string.Empty;
    public string CompanyAboutUs { get; init; } = string.Empty;
    public Guid CompanyJobCategoryId { get; set; }
    public string CompanyJobCategoryName { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public DateTime? FeaturedUntil { get; set; }
    public DateTime CreatedAt { get; init; }
    public List<string> Skills { get; init; } = new List<string>();
    public Guid CityId { get; init; }
    public Guid CompanyId { get; init; }
}
