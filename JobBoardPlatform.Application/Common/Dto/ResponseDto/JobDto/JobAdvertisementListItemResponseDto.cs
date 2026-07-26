using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;

public class JobAdvertisementListItemResponseDto
{
    public Guid JobId { get; set; }

    public string JobName { get; set; } = string.Empty;

    public Guid AdvertisementId { get; set; }

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

    public string AboutCompany { get; set; } = string.Empty;

    public Guid CompanyJobCategoryId { get; set; }

    public string CompanyJobCategoryName { get; set; } = string.Empty;

    public List<string> SkillNames { get; set; } = new();
}
