using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;

public record UpdateEducationDetailRequestDto(
    CertificateDegree? CertificateDegree,

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Major must be between 2 and 100 characters.")]
    string? Major,

    [StringLength(200, MinimumLength = 2, ErrorMessage = "University must be between 2 and 200 characters.")]
    string? University,

    DateTime? StartDate,

    DateTime? CompletionDate,

    [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100.")]
    double? Percentage,

    bool? IsCurrentlyStudying
);


