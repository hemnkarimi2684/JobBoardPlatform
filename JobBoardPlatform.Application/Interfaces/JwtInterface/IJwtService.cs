using JobBoardPlatform.Application.Common.Dto.AuthenticationDto.Result;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Application.Interfaces.JwtInterface;

public interface IJwtService
{
    Task<TokenLoginResult> GenerateTokenAsync(User user);
}
