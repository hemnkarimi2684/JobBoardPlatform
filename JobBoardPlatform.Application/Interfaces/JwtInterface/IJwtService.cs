using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Application.Interfaces.JwtInterface;

public interface IJwtService
{
    Task<TokenLoginResponseDto> GenerateTokenAsync(User user);
}
