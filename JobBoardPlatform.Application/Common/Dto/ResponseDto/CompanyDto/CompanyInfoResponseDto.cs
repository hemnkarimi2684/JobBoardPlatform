using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public class CompanyInfoResponseDto
{
    public string Name { get; init; } = default!;
    public Guid UserId { get; init; }
    public DateTime YearOfEstablishment { get; init; }
    public string Industry { get; init; } = default!;
    public string AboutUs { get; init; } = default!;
    public string WebSiteAddress { get; init; } = default!;
    public OwnershipType OwnershipType { get; init; }
    public CompanySizeEnum CompanySize { get; init; }
    public string? ActivityType { get; init; }
    public Guid? CompanyImageFileId { get; init; }
}

