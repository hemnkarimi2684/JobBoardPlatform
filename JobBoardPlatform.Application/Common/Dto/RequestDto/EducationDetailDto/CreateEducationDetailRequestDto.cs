using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;

public record CreateEducationDetailRequestDto(
    [Required(ErrorMessage = "Certificate degree name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Certificate degree name must be between 2 and 100 characters.")]
    string CertificateDegreeName,

    [Required(ErrorMessage = "Major is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Major must be between 2 and 100 characters.")]
    string Major,

    [Required(ErrorMessage = "University is required.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "University must be between 2 and 200 characters.")]
    string University,

    [Required(ErrorMessage = "Start date is required.")]
    DateTime StartDate,

    DateTime? CompletionDate,

    [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
    double? Percentage,

    [Required(ErrorMessage = "IsCurrentlyStudying is required.")]
    bool IsCurrentlyStudying,

    [Required(ErrorMessage = "UserId is required.")]
    Guid UserId
);