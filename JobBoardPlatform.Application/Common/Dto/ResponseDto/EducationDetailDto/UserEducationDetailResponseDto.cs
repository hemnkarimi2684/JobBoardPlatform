using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;

public record UserEducationDetailResponseDto(
    CertificateDegree CertificateDegreeName,
    string Major,
    string University,
    DateTime StartDate,
    DateTime? CompletionDate,
    int? Percentage,
    bool IsCurrentlyStudying
);

