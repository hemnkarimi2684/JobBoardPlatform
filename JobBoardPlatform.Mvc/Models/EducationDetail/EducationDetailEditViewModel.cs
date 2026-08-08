using JobBoardPlatform.Application.Common.Dto.RequestDto.EducationDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;

namespace JobBoardPlatform.Mvc.Models.EducationDetail;

public class EducationDetailEditViewModel : UpdateEducationDetailRequestDto
{
    public static EducationDetailEditViewModel FromResponseDto(EducationHistoryResponseDto source)
        => new()
        {
            CertificateDegree = source.CertificateDegreeName,
            Major = source.Major,
            University = source.University,
            StartDate = source.StartDate,
            CompletionDate = source.CompletionDate,
            Percentage = source.Percentage,
            IsCurrentlyStudying = source.IsCurrentlyStudying
        };
}
