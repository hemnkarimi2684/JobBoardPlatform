using JobBoardPlatform.Application.Common.Dto.RequestDto.CompanyDto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.AuthenticationDto;

public class RegisterEmployerRequestDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(
        @"^(?:\+98|0)9\d{9}$",
        ErrorMessage = "Phone number must start with 09 or +98 and be a valid Iranian mobile number."
    )]
    public string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).+$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character."
    )]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "the Name is required", AllowEmptyStrings = false)]
    [MinLength(2, ErrorMessage = "the Name characteers cannot be lower than 2")]
    [MaxLength(120, ErrorMessage = "the Name characteers cannot be higher than 120")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "the YearOfEstablishment is required", AllowEmptyStrings = false)]
    [DataType(DataType.Date)]
    public DateTime YearOfEstablishment { get; set; }

    [Required(ErrorMessage = "the Industry is required", AllowEmptyStrings = false)]
    [MinLength(2, ErrorMessage = "the Industry characteers cannot be lower than 2")]
    [MaxLength(200, ErrorMessage = "the Industry characteers cannot be higher than 200")]
    public string Industry { get; set; } = default!;

    [Required(ErrorMessage = "the AboutUs is required", AllowEmptyStrings = false)]
    [MinLength(50, ErrorMessage = "the AboutUs characteers cannot be lower than 50")]
    [MaxLength(1500, ErrorMessage = "the AboutUs characteers cannot be higher than 1500")]
    public string AboutUs { get; set; } = default!;

    [Required(ErrorMessage = "the WebSiteAddress is required", AllowEmptyStrings = false)]
    [MinLength(2, ErrorMessage = "the WebSiteAddress characteers cannot be lower than 2")]
    [MaxLength(100, ErrorMessage = "the WebSiteAddress characteers cannot be higher than 100")]
    public string WebSiteAddress { get; set; } = default!;

    [Required(ErrorMessage = "the OwnershipType is required", AllowEmptyStrings = false)]
    public OwnershipType OwnershipType { get; set; } = default!;

    [Required(ErrorMessage = "the CompanySize is required", AllowEmptyStrings = false)]
    public CompanySizeEnum CompanySize { get; set; } = default!;

    [Required(ErrorMessage = "the CityId is required", AllowEmptyStrings = false)]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "the Location is required", AllowEmptyStrings = false)]
    [MinLength(1, ErrorMessage = "the Location characteers cannot be lower than 5")]
    [MaxLength(2000, ErrorMessage = "the Location characteers cannot be higher than 200")]
    public string Location { get; set; } = default!;

    [MinLength(2, ErrorMessage = "the ActivityType characteers cannot be lower than 100")]
    [MaxLength(120, ErrorMessage = "the ActivityType characteers cannot be higher than 2000")]
    public string? ActivityType { get; set; } = null;

    public CreateCompanyRequestDto ToCreateCompanyRequestDto(Guid ownerByUserId)
    {
        return new CreateCompanyRequestDto(
            Name,
            YearOfEstablishment,
            Industry,
            AboutUs,
            WebSiteAddress,
            OwnershipType,
            ownerByUserId,
            CompanySize,
            CityId,
            Location,
            ActivityType
        );
    }
}
