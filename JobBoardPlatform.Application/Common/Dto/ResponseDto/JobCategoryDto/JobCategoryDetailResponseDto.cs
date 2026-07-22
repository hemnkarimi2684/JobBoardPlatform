using JobBoardPlatform.Application.Common.Dto.ResponseDto.JobDto;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.JobCategoryDto;

public class JobCategoryDetailResponseDto
{
    public Guid JobCategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<JobListItemResponseDto> Jobs { get; set; } = new();
}
