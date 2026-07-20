namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

public class JwtSettings
{
    public string Audience { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public string Secret { get; set; } = string.Empty;

    public string EncryptKey { get; set; } = string.Empty;

    public int TokenLifeTime { get; set; }
}
