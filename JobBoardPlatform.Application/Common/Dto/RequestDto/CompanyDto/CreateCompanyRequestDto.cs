using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;

public class CreateCompanyRequestDto
{
    [Required(ErrorMessage = "Company name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 120 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Year of establishment is required.")]
    [DataType(DataType.Date)]
    public DateTime YearOfEstablishment { get; set; }

    [Required(ErrorMessage = "About Us description is required.")]
    [StringLength(1500, MinimumLength = 50, ErrorMessage = "About Us must be between 50 and 1500 characters.")]
    public string AboutUs { get; set; } = string.Empty;

    [Required(ErrorMessage = "Website address is required.")]
    [Url(ErrorMessage = "Invalid website URL format.")]
    [StringLength(100, ErrorMessage = "Website address cannot exceed 100 characters.")]
    public string WebSiteAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ownership type is required.")]
    [EnumDataType(typeof(OwnershipType))]
    public OwnershipType OwnershipType { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid OwnedByUserId { get; set; }

    [Required(ErrorMessage = "Company size is required.")]
    [EnumDataType(typeof(CompanySizeEnum))]
    public CompanySizeEnum CompanySize { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid JobCategoryId { get; set; }

    [Required(ErrorMessage = "Location address is required.")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "Location must be between 5 and 2000 characters.")]
    public string Location { get; set; } = string.Empty;

    [StringLength(120, MinimumLength = 2, ErrorMessage = "Activity type must be between 2 and 120 characters.")]
    public string? ActivityType { get; set; }
}


