using JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

namespace JobBoardPlatform.Mvc.Models.Resume;

public class ResumeIndexViewModel : ResumeDetailResponseDto
{
    public static ResumeIndexViewModel FromResponseDto(ResumeDetailResponseDto source)
        => new()
        {
            ResumeId = source.ResumeId,
            UserId = source.UserId,
            ResumeFileId = source.ResumeFileId,
            Title = source.Title,
            ResumeUserProfiles = source.ResumeUserProfiles,
            ResumeEducationDetails = source.ResumeEducationDetails,
            ResumeExperienceDetails = source.ResumeExperienceDetails,
            ResumeSkills = source.ResumeSkills
        };
}
