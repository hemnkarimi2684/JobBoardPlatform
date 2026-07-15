namespace JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Result;

public record TokenLoginResult(string AccessToken, TimeSpan ExpieryTime, string TokenType);

