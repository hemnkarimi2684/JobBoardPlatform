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

    /// <summary>
    /// مقدار رفرش توکن
    /// </summary>
    public string Token { get; private set; }

    /// <summary>
    /// تاریخ انقضا رفرش توکن 
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// منقضی شده یا نه
    /// </summary>
    public bool IsRevoked { get; private set; }

    /// <summary>
    /// کی منقضی شده؟
    /// </summary>
    public DateTime? RevokedAt { get; private set; }

    /// <summary>
    /// ایا هنوز فعاله
    /// </summary>
    public bool IsActive => !IsRevoked && RevokedAt is null && DateTime.UtcNow < ExpiresAt;

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به کاربری که این رفرش توکن رو داره 
    /// </summary>
    public Guid UserId { get; private set; }

    #endregion

    #region Navigation properties

    /// <summary>
    /// جزئیات مربوط به کاربری که این رفرش توکن رو داره 
    /// </summary>
    public virtual User User { get; private set; }

    #endregion

    /// <summary>
    /// متدی برای منقضی کردن رفرش توکن 
    /// </summary>
    public void Revoke()
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
    }

    protected override void Validate() => throw new NotImplementedException();
}
