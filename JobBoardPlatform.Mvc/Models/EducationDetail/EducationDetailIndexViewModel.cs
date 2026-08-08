using JobBoardPlatform.Application.Common.Dto.ResponseDto.EducationDetailDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.EducationDetail;

public class EducationDetailIndexViewModel : Pagination<EducationHistoryResponseDto>
{
    public static EducationDetailIndexViewModel FromResponseDto(Pagination<EducationHistoryResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
