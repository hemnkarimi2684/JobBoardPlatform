using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;

namespace JobBoardPlatform.Core.Entities.SkillEntity.Entity;

/// <summary>
/// مهارت
/// </summary>
public class Skill : BaseEntity
{
    private Skill() { }
    
    public Skill(string name, Guid? createdById = null)
    {
        Name = name;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// اسم مهارت
    /// </summary>
    public string Name { get; private set; }

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به مهارت خواسته شده در اگهی های شغلی 
    /// </summary>
    public virtual ICollection<AdvertisementSkill> AdvertisementSkills { get; private set; } = new List<AdvertisementSkill>();

    /// <summary>
    /// جزئیات مربوط به مهارت های کاربر 
    /// </summary>
    public virtual ICollection<UserSkill> UserSkills { get; private set; } = new List<UserSkill>();

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.SkillNameIsRequired);

        if (Name.Length < 2 || Name.Length > 100)
            throw new DomainException(DomainErrors.SkillNameInvalidLength);
    }

}
