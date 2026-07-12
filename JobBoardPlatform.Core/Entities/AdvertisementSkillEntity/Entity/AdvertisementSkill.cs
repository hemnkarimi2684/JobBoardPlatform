using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.SkillEntity.Entity;

namespace JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;

public class AdvertisementSkill : BaseEntity
{
    private AdvertisementSkill() { }

    public AdvertisementSkill(Guid advertisementId, Guid skillId, Guid? createdById = null)
    {
        AdvertisementId = advertisementId;
        SkillId = skillId;
        CreatedById = createdById;
    }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به اگهی که دارای مهارته
    /// </summary>
    public Guid AdvertisementId { get; private set; }

    /// <summary>
    /// شناسه مربوط به مهارتی که در اگهی قرار دارد
    /// </summary>
    public Guid SkillId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به اگهی دارای مهارت
    /// </summary>
    public virtual Advertisement Advertisement { get; private set; }

    /// <summary>
    /// جزئیات مربوط به مهارتی که در اگهی ذکر شده 
    /// </summary>
    public virtual Skill Skill { get; private set; }

    #endregion

    protected override void Validate() => throw new NotImplementedException();
    
}
