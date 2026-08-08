using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;

namespace JobBoardPlatform.Mvc.Models.JobCategory;

public class JobCategoryDetailsViewModel : JobCategoryDetailResponseDto
{
    public static JobCategoryDetailsViewModel FromResponseDto(JobCategoryDetailResponseDto source)
        => new()
        {
            JobCategoryId = source.JobCategoryId,
            Name = source.Name,
            Jobs = source.Jobs
        };
}
