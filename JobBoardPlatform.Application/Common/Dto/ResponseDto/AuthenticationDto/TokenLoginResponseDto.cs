namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;

public record TokenLoginResponseDto(string AccessToken, TimeSpan ExpieryTime, string TokenType);

