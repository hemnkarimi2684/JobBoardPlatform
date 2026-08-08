using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobApplicationDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Mvc.Models.JobApplication;

public class JobApplicationMyViewModel : Pagination<JobApplicationDetailResponseDto>
{
    public static JobApplicationMyViewModel FromResponseDto(Pagination<JobApplicationDetailResponseDto> source)
        => new()
        {
            Data = source.Data,
            PageNumber = source.PageNumber,
            PageSize = source.PageSize,
            TotalPageCount = source.TotalPageCount
        };
}
