using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;

namespace JobBoardPlatform.Core.Entities.CompanyEntity.Dto;

public record CompanyInfoUpdate(
    string? Name, 
    DateTime? YearOfEstablishment, 
    Guid? JobCategoryId,
    string? AboutUs, 
    string? WebSiteAddress, 
    OwnershipType? OwnershipType,
    CompanySizeEnum? CompanySize,
    string? ActivityType,
    Guid? ModifiedById);

