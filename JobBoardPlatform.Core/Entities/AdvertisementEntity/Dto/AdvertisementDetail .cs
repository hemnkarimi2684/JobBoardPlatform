using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;

public class AdvertisementDetail
{
    public int MinimumAge { get; init; }
    public int MaximumAge { get; init; }
    public int ExperienceLevel { get; init; }

    public decimal MinimumSalary { get; init; }
    public decimal MaximumSalary { get; init; }

    public string Description { get; init; } = string.Empty;
    public string CityName { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string JobName { get; init; } = string.Empty;
    public string CompanyAboutUs { get; init; } = string.Empty;
    public string CompanyJobCategoryName { get; init; } = string.Empty;

    public bool IsFeatured { get; init; }
    public bool IsActive { get; init; }

    public DateTime? FeaturedUntil { get; init; }
    public DateTime CreatedAt { get; init; }

    public Guid AdvertisementId { get; init; }
    public Guid JobId { get; init; }
    public Guid CompanyJobCategoryId { get; init; }
    public Guid CityId { get; init; }
    public Guid CompanyId { get; init; }
    public Guid EmployerUserId { get; init; }
    public Guid? CompanyImageFileId { get; init; }

    public CollaborationType CollaborationType { get; init; }

    public AdvertisementStatus Status { get; init; }

    public List<string> Skills { get; init; } = new List<string>();
}
