using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Data;
using JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;
using JobBoardPlatform.Infrastructure.Data;
using JobBoardPlatform.Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace JobBoardPlatform.Infrastructure.Repositories.RefreshTokenRepo;

public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<RefreshToken>> GetAllActiveTokensAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Entities
                          .Where(rt => rt.UserId == userId && !rt.IsRevoked && rt.RevokedAt == null)
                          .ToListAsync(cancellationToken);
    }

    public async Task<RefreshToken?> GetRefreshTokenByTokenAsync(string token, CancellationToken cancellationToken, bool tracking = false)
    {
        var query = Entities.AsQueryable();

        if (!tracking)
            query = query.AsNoTracking();

        return await query
                        .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }
}
