using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;

public class CreateEducationDetailRequestDto
{
    [EnumDataType(typeof(CertificateDegree))]
    public CertificateDegree CertificateDegree { get; set; }

    [Required(ErrorMessage = "Major is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Major must be between 2 and 100 characters.")]
    public string Major { get; set; } = string.Empty;

    [Required(ErrorMessage = "University is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "University must be between 2 and 200 characters.")]
    public string University { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    public DateTime? CompletionDate { get; set; }

    [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
    public double? Percentage { get; set; }

    public bool IsCurrentlyStudying { get; set; }

    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid UserId { get; set; }
}