using JobBoardPlatform.Application.Implementation.CompanyBusiness;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.CompanyDto.Command;

public record UpdateCompanyInfoCommand(
    string? Name,
    DateTime? YearOfEstablishment,
    string? Industry,
    string? AboutUs,
    string? WebSiteAddress,
    string? OwnershipType,
    string? CompanySize,
    string? ActivityType
    );
