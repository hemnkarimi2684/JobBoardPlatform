using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.CompanyDto;

public record CompanyInfoResponseDto(
    string Name, 
    DateTime YearOfEstablishment,
    string Industry, 
    string AboutUs,
    string WebSiteAddress,
    OwnershipType OwnershipType, 
    CompanySizeEnum CompanySize,
    string? ActivityType);

