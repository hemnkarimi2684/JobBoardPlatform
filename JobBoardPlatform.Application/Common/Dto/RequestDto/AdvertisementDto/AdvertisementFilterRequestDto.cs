using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AdvertisementDto;

public class AdvertisementFilterRequestDto
{
    public Guid? JobCategoryId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "MinimumSalary must be greater than or equal to 0.")]
    public decimal? MinimumSalary { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "MaximumSalary must be greater than or equal to 0.")]
    public decimal? MaximumSalary { get; set; }

    [EnumDataType(typeof(CollaborationType))]
    public CollaborationType? CollaborationType { get; set; }

    public List<Guid>? SkillIds { get; set; }

    public AdvertisementQueryFilter MaoToQueryFilter()
    {
        return new AdvertisementQueryFilter
        {
            JobCategoryId = JobCategoryId,
            MaximumSalary = MaximumSalary,
            MinimumSalary = MinimumSalary,
            SkillIds = SkillIds
        };
    }
}
