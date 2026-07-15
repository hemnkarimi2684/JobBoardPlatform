using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;

public record CreateEducationDetailRequestDto(
    string CertificateDegreeName, 
    string Major, 
    string University,
    DateTime StartDate,
    DateTime? CompletionDate, 
    int? Percentage, 
    bool IsCurrentlyStudying, 
    Guid UserId
    );
