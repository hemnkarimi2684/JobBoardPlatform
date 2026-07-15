namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AdminDto;

public class AdminData
{
    public string Role { get; set; }
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
