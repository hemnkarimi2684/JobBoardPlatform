namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;

public class UserSkillResponseDto
{
    public Guid SkillId { get; init; }

    public string SkillName { get; init; } = string.Empty;

    public Guid UserId { get; init; }
}
