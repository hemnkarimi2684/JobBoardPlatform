using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public record ResumeEducationDetailResponseDto(
    Guid EducationDetailId,
    CertificateDegree CertificateDegreeName,
    string Major,
    string University,
    DateTime StartDate,
    DateTime? CompletionDate,
    double? Percentage,
    bool IsCurrentlyStudying
);

