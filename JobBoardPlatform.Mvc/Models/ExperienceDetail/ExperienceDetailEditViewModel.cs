using JobBoardPlatform.Application.Common.Dto.RequestDto.ExperienceDetailDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.ExperienceDetailDto;

namespace JobBoardPlatform.Mvc.Models.ExperienceDetail;

public class ExperienceDetailEditViewModel : UpdateExperienceDetailRequestDto
{
    public static ExperienceDetailEditViewModel FromResponseDto(ExperienceHistoryResponseDto source)
        => new()
        {
            LastJobTitle = source.LastJobTitle,
            SeniorityLevel = source.SeniorityLevel,
            JobCategory = source.JobCategory,
            City = source.City,
            StartDate = source.StartDate,
            EndDate = source.EndDate,
            IsCurrentJob = source.IsCurrentJob
        };
}
