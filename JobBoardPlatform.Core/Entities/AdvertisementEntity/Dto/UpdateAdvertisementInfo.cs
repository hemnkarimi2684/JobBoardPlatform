using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;

public class UpdateAdvertisementInfo
{
    public string? Description { get; init; }

    public int? MinimumAge { get; init; }
    public int? MaximumAge { get; init; }
    public int? ExperienceLevel { get; init; }

    public decimal? MinimumSalary { get; init; }
    public decimal? MaximumSalary { get; init; }

    public CollaborationType? CollaborationType { get; init; }

    public Guid? ModifiedById { get; init; }
}
