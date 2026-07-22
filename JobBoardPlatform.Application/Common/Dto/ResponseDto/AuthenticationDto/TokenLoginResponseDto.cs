namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

public class TokenLoginResponseDto
{
    public Guid UserId { get; set; }

    public string AccessToken { get; init; } = string.Empty;

    public string RefreshToken { get; init; } = string.Empty;

    public TimeSpan AccessTokenExpiryTime { get; init; }

    public DateTime RefreshTokenExpiryTime { get; init; }

    public string TokenType { get; init; } = string.Empty;
}

