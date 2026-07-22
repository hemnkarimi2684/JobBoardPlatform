using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;

namespace JobBoardPlatform.Core.Entities.RefreshTokenEntity.Data;

public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
{
    /// <summary>
    /// دریافت تمام رفرش توکن های فعال کاربر
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<RefreshToken>> GetAllActiveTokensAsync(Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// دریافت رفرش توکن توسط توکن 
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken, bool tracking = false);
}
