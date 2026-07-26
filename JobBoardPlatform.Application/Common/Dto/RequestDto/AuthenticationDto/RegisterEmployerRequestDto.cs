using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public class RegisterEmployerRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(
        @"^(?:\+98|0)9\d{9}$",
        ErrorMessage = "Phone number must start with 09 or +98 and be a valid Iranian mobile number."
    )]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
    )]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Name is required.")]
    [StringLength(120, MinimumLength = 2, ErrorMessage = "The Name characters cannot be less than 2 or more than 120.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The YearOfEstablishment is required.")]
    [DataType(DataType.Date)]
    public DateTime YearOfEstablishment { get; set; }

    [Required(ErrorMessage = "The AboutUs is required.")]
    [StringLength(1500, MinimumLength = 50, ErrorMessage = "The AboutUs characters cannot be less than 50 or more than 1500.")]
    public string AboutUs { get; set; } = string.Empty;

    [Required(ErrorMessage = "The WebSiteAddress is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "The WebSiteAddress characters cannot be less than 2 or more than 100.")]
    public string WebSiteAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "The OwnershipType is required.")]
    public OwnershipType OwnershipType { get; set; }

    [Required(ErrorMessage = "The CompanySize is required.")]
    public CompanySizeEnum CompanySize { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid JobCategoryId { get; set; }

    [Required(ErrorMessage = "The Location is required.")]
    [StringLength(2000, MinimumLength = 5, ErrorMessage = "The Location characters cannot be less than 5 or more than 2000.")]
    public string Location { get; set; } = string.Empty;

    [StringLength(120, ErrorMessage = "The ActivityType characters cannot be more than 120.")]
    public string? ActivityType { get; set; }

    public CreateCompanyRequestDto ToCreateCompanyRequestDto(Guid ownerByUserId)
    {
        return new CreateCompanyRequestDto
        {
            Name = Name,
            YearOfEstablishment = YearOfEstablishment,
            AboutUs = AboutUs,
            WebSiteAddress = WebSiteAddress,
            OwnershipType = OwnershipType,
            OwnedByUserId = ownerByUserId,
            CompanySize = CompanySize,
            CityId = CityId,
            Location = Location,
            ActivityType = ActivityType,
            JobCategoryId = JobCategoryId
        };
    }
}
