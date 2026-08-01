using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class CompanyDetailResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime YearOfEstablishment { get; set; }

    public Guid JobCategoryId { get; set; }

    public string JobCategoryName { get; set; } = null!;

    public string? AboutUs { get; set; }

    public string? WebSiteAddress { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OwnershipType OwnershipType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CompanySizeEnum CompanySize { get; set; }

    public string? ActivityType { get; set; }

    public Guid? CompanyImageFileId { get; set; }

    public List<Guid> Cities { get; set; } = new();
}
