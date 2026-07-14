using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;

public record UpdateEducationDetail(
    CertificateDegree? CertificateDegreeName,
    string? Major,
    string? University,
    DateTime? StartDate,
    DateTime? CompletionDate,
    int? Percentage,
    bool? IsCurrentlyStudying,
    Guid? ModifiedById = null
);

