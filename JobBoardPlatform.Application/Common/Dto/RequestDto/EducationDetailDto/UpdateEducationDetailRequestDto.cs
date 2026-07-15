using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;

public record UpdateEducationDetailRequestDto(
    string? CertificateDegreeName,
    string? Major,
    string? University,
    DateTime? StartDate,
    DateTime? CompletionDate,
    int? Percentage,
    bool? IsCurrentlyStudying
);

