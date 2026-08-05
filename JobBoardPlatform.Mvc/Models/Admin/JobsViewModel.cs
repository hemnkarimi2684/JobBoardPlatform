using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;

namespace JobBoardPlatform.Mvc.Models.Admin;

public class JobsViewModel
{
    public List<JobResponseDto> Jobs { get; set; } = new();

    public static JobsViewModel FromResponseDto(List<JobResponseDto> jobs)
        => new() { Jobs = jobs };
}
