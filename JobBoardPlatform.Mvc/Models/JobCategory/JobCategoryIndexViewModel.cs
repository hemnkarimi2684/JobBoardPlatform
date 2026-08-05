using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.JobCategory;

public class JobCategoryIndexViewModel : Pagination<JobCategoryResponseDto>
{
    public static JobCategoryIndexViewModel FromResponseDto(Pagination<JobCategoryResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
