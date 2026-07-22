using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;

public class AdvertisementQueryFilter
{
    public Guid? JobCategoryId { get; set; }

    public decimal? MinimumSalary { get; set; }

    public decimal? MaximumSalary { get; set; }

    public CollaborationType? CollabrationType { get; set; }

    public List<Guid>? SkillIds { get; set; }
}
