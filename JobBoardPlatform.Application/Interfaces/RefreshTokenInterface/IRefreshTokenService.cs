using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Application.Interfaces.RefreshTokenInterface;

public interface IRefreshTokenService
{
    /// <summary>
    /// ساخت رفرش توکن
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RefreshToken> CreateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// منقضی کردن تمام توکن های قبلی کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RevokeAllActiveTokensAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت رفرش توکن توسط توکن 
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<RefreshToken> GetRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken = default, bool tracking = false);

    /// <summary>
    /// منقضی کردن یک رفرش توکن
    /// </summary>
    /// <param name="token"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> RevokeAsync(string token, CancellationToken cancellationToken = default);
}
