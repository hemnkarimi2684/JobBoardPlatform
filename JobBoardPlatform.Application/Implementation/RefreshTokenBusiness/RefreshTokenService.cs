using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.AuthenticationDto;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.JwtInterface;
using JobBoardPlatform.Application.Interfaces.RefreshTokenInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace JobBoardPlatform.Application.Implementation.RefreshTokenBusiness;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly JwtSettings _jwtSettings;

    public RefreshTokenService(IUnitOfWork unitOfWork, IOptions<JwtSettings> options, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _jwtSettings = options.Value;
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var token = GenerateRefreshToken();

        var refreshExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifeTime);

        var refreshToken = new RefreshToken(token, refreshExpiresAt, userId);

        await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return refreshToken;
    }

    public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken = default, bool tracking = false)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetRefreshTokenByTokenAsync(token, cancellationToken, tracking);

        if (refreshToken == null)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        return refreshToken;
    }

    public async Task RevokeAllActiveTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var activeRefreshTokens = await _unitOfWork.RefreshTokenRepository.GetAllActiveTokensAsync(userId, cancellationToken);

        foreach (var activeRefreshToken in activeRefreshTokens)
        {
            activeRefreshToken.Revoke();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> RevokeAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = await _unitOfWork.RefreshTokenRepository.GetRefreshTokenByTokenAsync(token, cancellationToken, true);

        if (refreshToken == null || !refreshToken.IsActive)
            throw new NotFoundException("Token not found or already inactive.");

        refreshToken.Revoke();

        return await _unitOfWork.SaveChangesAsync(cancellationToken) > 0;
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}
