using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;
using JobBoardPlatform.Core.Entities.UserSkillEntity.Entity;
using Microsoft.AspNetCore.Identity;

namespace JobBoardPlatform.Core.Entities.UserEntity.Entity;

public class User : IdentityUser<Guid>, IEntity
{
    private User() { }

    public User(string email, string phoneNumber, bool isApproved, Guid? createdById = null)
    {
        Email = email;
        PhoneNumber = phoneNumber;
        UserName = Email;
        IsApproved = isApproved;
        CreatedById = createdById;

        IsActive = true;

        //Methods
        PhoneNumber.FixPhoneNumberFormat();
        Validate();
    }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsApproved { get; private set; }

    public bool IsActive { get; private set; }

    #region Foreign Keys

    public Guid? CreatedById { get; private set; }

    public Guid? ModifiedById { get; private set; }

    public Guid? DeletedById { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// نویگیشن پراپرتی مربوط به سازمان کارفرما
    /// </summary>
    public virtual Company? Company { get; private set; }

    /// <summary>
    /// جزئیات مربوط به مدارک تحصیلی کاربر
    /// </summary>
    public virtual ICollection<EducationDetail> EducationDetails { get; private set; } = new List<EducationDetail>();

    /// <summary>
    /// جزئیات مربوط به تجربه کاری کاربر
    /// </summary>
    public virtual ICollection<ExperienceDetail> ExperienceDetails { get; private set; } = new List<ExperienceDetail>();

    /// <summary>
    /// جزئیات مربوط به رزومه کاربر
    /// </summary>
    public virtual Resume? Resume { get; private set; }

    /// <summary>
    /// جزئیات مربوط به مهارت های کاربر
    /// </summary>
    public virtual ICollection<UserSkill> UserSkills { get; private set; } = new List<UserSkill>();

    /// <summary>
    /// جزئیات مربوط به درخواست کاری کاربر
    /// </summary>
    public virtual ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();

    /// <summary>
    /// جزئیات مربوط به پرداخت های کارفرما
    /// </summary>
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    /// <summary>
    /// جزئیات مربوط به اطلاعات مربوط به پروفایل کاربر
    /// </summary>
    public virtual UserProfile? UserProfile { get; private set; }

    public User? Creator { get; private set; }

    public User? Modifier { get; private set; }

    public User? Deleter { get; private set; }

    #endregion

    private void Validate()
    {
        Email?.EmailIsValid();

        PhoneNumber?.PhoneNumberIsValid();
    }

    public void SoftDelete(Guid? deletedById)
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        ModifiedAt = DateTime.UtcNow;
        DeletedById = deletedById;
    }

    public void Update(Guid? modifiedById)
    {
        ModifiedById = modifiedById;
        ModifiedAt = DateTime.UtcNow;
    }

    public void UpdateIsApproved(bool isApproved, Guid? modifiedById)
    {
        IsApproved = isApproved;
        Update(modifiedById);
    }

    public void UpdateIsActive(bool isActive, Guid? modifiedById)
    {
        IsActive = isActive;
        Update(modifiedById);
    }
}
