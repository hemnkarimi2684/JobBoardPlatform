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

    public User(string email, string phoneNumber, string passwordHash)
    {
        Email = email;
        PhoneNumber = phoneNumber;
        PasswordHash = passwordHash;

        //Methods
        PhoneNumber.FixPhoneNumberFormat();
        Validate();
    }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted { get; private set; }

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

    #endregion

    private void Validate()
    {
        Email?.EmailIsValid();

        PhoneNumber?.PhoneNumberIsValid();

        if (string.IsNullOrWhiteSpace(PasswordHash))
            throw new DomainException(DomainErrors.PasswordHashIsRequired);
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Update() => ModifiedAt = DateTime.UtcNow;

}
