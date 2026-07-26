namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.UserDto;

public class JobSeekerResponseDto
{
    public Guid JobSeekerId { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public Guid? JobSeekerProfileId { get; set; }
}
