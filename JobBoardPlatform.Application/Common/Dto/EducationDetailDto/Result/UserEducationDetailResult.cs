using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Result;

public record UserEducationDetailResult(
    CertificateDegree CertificateDegreeName,
    string Major,
    string University,
    DateTime StartDate,
    DateTime? CompletionDate,
    int? Percentage,
    bool IsCurrentlyStudying
);

