using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class CompanyProfileResponseDto
{
    public string Name { get; init; } = default!;

    public Guid UserId { get; init; }

    public DateTime YearOfEstablishment { get; init; }

    public string Industry { get; init; } = default!;

    public string AboutUs { get; init; } = default!;

    public string WebSiteAddress { get; init; } = default!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OwnershipType OwnershipType { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CompanySizeEnum CompanySize { get; init; }

    public string? ActivityType { get; init; }

    public Guid? CompanyImageFileId { get; init; }
}

