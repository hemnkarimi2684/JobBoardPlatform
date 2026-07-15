namespace JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Command;

public record RegisterCommand(string Email, string PhoneNumber, string Password);

