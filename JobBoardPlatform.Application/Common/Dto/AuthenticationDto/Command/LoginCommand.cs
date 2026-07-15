namespace JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Command;

public record LoginCommand(string EmailOrPhoneNumber, string Password);

