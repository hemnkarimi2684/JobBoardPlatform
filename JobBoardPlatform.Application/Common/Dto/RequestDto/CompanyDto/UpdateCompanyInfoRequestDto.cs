using JobBoardPlatform.Application.Implementation.CompanyBusiness;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;

public class UpdateCompanyInfoRequestDto
{
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 120 characters.")]
    public string? Name { get; set; }

    [DataType(DataType.Date)]
    public DateTime? YearOfEstablishment { get; set; }

    [StringLength(1500, MinimumLength = 50, ErrorMessage = "About Us must be between 50 and 1500 characters.")]
    public string? AboutUs { get; set; }

    [Url(ErrorMessage = "Invalid website URL format.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Website address must be between 2 and 100 characters.")]
    public string? WebSiteAddress { get; set; }

    [EnumDataType(typeof(OwnershipType))]
    public OwnershipType? OwnershipType { get; set; }

    [EnumDataType(typeof(CompanySizeEnum))]
    public CompanySizeEnum? CompanySize { get; set; }

    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid? JobCategoryId { get; set; }

    [StringLength(120, MinimumLength = 2, ErrorMessage = "Activity type must be between 2 and 120 characters.")]
    public string? ActivityType { get; set; }
}
