using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.CompanyDto.Command;

public record CreateCompanyCommand(
    string Name,
    DateTime YearOfEstablishment,
    string Industry,
    string AboutUs,
    string WebSiteAddress,
    string OwnershipType,
    Guid OwnedByUserId,
    string CompanySize,
    Guid CityId,
    string Location,
    string? ActivityType = null,
    Guid? CompanyImageFileId = null
);


