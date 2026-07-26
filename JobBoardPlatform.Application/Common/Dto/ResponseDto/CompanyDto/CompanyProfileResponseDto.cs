using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class CompanyProfileResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = default!;

    public Guid UserId { get; init; }

    public DateTime YearOfEstablishment { get; init; }

    public string JobCategoryName { get; init; } = string.Empty;

    public Guid JobCategoryId { get; init; }

    public string AboutUs { get; init; } = default!;

    public string WebSiteAddress { get; init; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OwnershipType OwnershipType { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CompanySizeEnum CompanySize { get; init; }

    public string? ActivityType { get; init; }

    public Guid? CompanyImageFileId { get; init; }

    public List<Guid> Cities { get; init; } = new();
}

