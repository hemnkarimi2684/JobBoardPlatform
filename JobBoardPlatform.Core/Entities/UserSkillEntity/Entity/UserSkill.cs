using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;

/// <summary>
/// جدول واسط بین کاربر و مهارت
/// </summary>
public class UserSkill : BaseEntity
{
    private UserSkill() { }
    
    public UserSkill(Guid userId, Guid skillId, Guid? createdById = null)
    {
        UserId = userId;
        SkillId = skillId;
        CreatedById = createdById;
    }

    #region Foreign Keys

    /// <summary>
    /// شناسه مورد نظر کاربر دارای مهارت
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// شناسه مورد نظر مهارت کاربر
    /// </summary>
    public Guid SkillId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به کاربر دارای مهارت
    /// </summary>
    public virtual User User { get; private set; }

    /// <summary>
    /// جزئیات مربوط به مهارت های کاربر
    /// </summary>
    public virtual Skill Skill { get; private set; }


    #endregion

    protected override void Validate() => throw new NotImplementedException();
}
