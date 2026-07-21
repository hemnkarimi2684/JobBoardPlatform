namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.SkillDto;

public class UserSkillDetailResponseDto
{
    public Guid SkillId { get; init; }

    public string SkillName { get; init; } = string.Empty;

    public Guid UserId { get; init; }
}
