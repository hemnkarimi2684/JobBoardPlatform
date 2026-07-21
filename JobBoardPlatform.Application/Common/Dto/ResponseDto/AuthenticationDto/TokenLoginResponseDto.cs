namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

public class TokenLoginResponseDto
{
    public string AccessToken { get; init; } = string.Empty;

    public TimeSpan ExpiryTime { get; init; }

    public string TokenType { get; init; } = string.Empty;
}

