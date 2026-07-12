using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.CompanyDto.Result;

public record CompanyInfoResult(string Name, DateTime YearOfEstablishment, string Industry, string AboutUs, string WebSiteAddress, OwnershipType OwnershipType, CompanySizeEnum CompanySize, string? ActivityType);

