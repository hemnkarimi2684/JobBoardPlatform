using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.ExperienceDetail;

public class ExperienceDetailIndexViewModel : Pagination<ExperienceHistoryResponseDto>
{
    public static ExperienceDetailIndexViewModel FromResponseDto(Pagination<ExperienceHistoryResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
