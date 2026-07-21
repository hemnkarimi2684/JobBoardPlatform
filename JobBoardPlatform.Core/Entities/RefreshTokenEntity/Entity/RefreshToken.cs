using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.RefreshTokenEntity.Entity;

public class RefreshToken : BaseEntity
{
    private RefreshToken() { }    

    public RefreshToken(string token, DateTime expiresAt, Guid userId)
    {
        Token = token;
        ExpiresAt = expiresAt;
        UserId = userId;
        IsRevoked = false;
    }

    public string Token { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public bool IsRevoked { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    #region Foriegn Keys

    public Guid UserId { get; private set; }

    #endregion

    #region Navigation properties

    public virtual User User { get; private set; }

    #endregion

    public void Revok()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }

    protected override void Validate() => throw new NotImplementedException();
}
