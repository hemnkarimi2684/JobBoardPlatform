using JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class JobSeekersViewModel : Pagination<JobSeekerDetailResponseDto>
{
    public static JobSeekersViewModel FromResponseDto(Pagination<JobSeekerDetailResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
