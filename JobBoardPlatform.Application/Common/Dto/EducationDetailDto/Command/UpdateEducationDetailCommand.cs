using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.EducationDetailDto.Command;

public record UpdateEducationDetailCommand(
    string? CertificateDegreeName,
    string? Major,
    string? University,
    DateTime? StartDate,
    DateTime? CompletionDate,
    int? Percentage,
    bool? IsCurrentlyStudying
);

