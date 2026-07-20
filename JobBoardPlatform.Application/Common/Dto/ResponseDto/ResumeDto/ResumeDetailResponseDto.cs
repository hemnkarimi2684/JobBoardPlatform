namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.ResumeDto;

public class ResumeDetailResponseDto
{
    public Guid? ResumeId { get; set; }

    public Guid UserId { get; set; }

    public Guid? ResumeFileId { get; set; }

    public string? Title { get; set; }

    public ResumeUserProfileResponseDto? ResumeUserProfiles { get; set; } 

    public List<ResumeEducationDetailResponseDto> ResumeEducationDetails { get; set; } = new();

    public List<ResumeExperienceDetailResponseDto> ResumeExperienceDetails { get; set; } = new();

    public List<ResumeSkillDetailResponseDto> ResumeSkills { get; set; } = new();
}
