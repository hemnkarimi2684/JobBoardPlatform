using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Job;

public class JobDetailsViewModel : Pagination<JobAdvertisementListItemResponseDto>
{
    public static JobDetailsViewModel FromResponseDto(Pagination<JobAdvertisementListItemResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
