using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.AdvertisementSkillEntity.Entity;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.JobEntity.Entity;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;

namespace JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;

/// <summary>
/// اگهی های شغلی
/// </summary>
public class Advertisement : BaseEntity
{
    private Advertisement() { }

    public Advertisement(string description, int minimumAge, int maximumAge, decimal minimumSalary, decimal maximumSalary, int experienceLevel, CollaborationType collaborationType, Guid jobId, Guid cityId, Guid companyId, Guid? createdById = null)
    {
        Description = description;
        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
        MinimumSalary = minimumSalary;
        MaximumSalary = maximumSalary;
        ExperienceLevel = experienceLevel;
        CollaborationType = collaborationType;
        JobId = jobId;
        CityId = cityId;
        CompanyId = companyId;
        CreatedById = createdById;
        IsActive = true;

        Validate();
    }

    /// <summary>
    /// شرح شغل و وظایف
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// مینیمم سن خواسته شده برای کار
    /// </summary>
    public int MinimumAge { get; private set; }

    /// <summary>
    /// ماکسیمم سن خواسته شده برای کار 
    /// </summary>
    public int MaximumAge { get; private set; }

    /// <summary>
    /// مینیمم حقوق پیشنهاد شده برای کار اگهی
    /// </summary>
    public decimal MinimumSalary { get; private set; }

    /// <summary>
    /// ماکسیمم حقوق پیشنهاد شده برای کار اگهی
    /// </summary>
    public decimal MaximumSalary { get; private set; }

    /// <summary>
    /// میزان سال های سابقه کاری 
    /// </summary>
    public int ExperienceLevel { get; private set; }

    /// <summary>
    /// نوع همکاری در کار
    /// </summary>
    public CollaborationType CollaborationType { get; private set; }

    /// <summary>
    /// وضعیت فعال یاغیر فعال بودن اگهی
    /// </summary>
    public bool IsActive { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به کار دارای اگهی
    /// </summary>
    public Guid JobId { get; private set; }

    /// <summary>
    /// شناسه مربوط به شهری که کمپانی در ان وجود داره
    /// </summary>
    public Guid CityId { get; private set; }

    /// <summary>
    /// شناسه مربوط به کمپانی که اگهی را داده
    /// </summary>
    public Guid CompanyId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به کار مورد نظر
    /// </summary>
    public virtual Job Job { get; private set; }

    /// <summary>
    /// جزئیات مربوط به شهری که کمپانی در ان قرار دارد
    /// </summary>
    public virtual City City { get; private set; }

    /// <summary>
    /// جزئیات مربوط به کمپانی که اگهی داده است
    /// </summary>
    public virtual Company Company { get; private set; }

    /// <summary>
    /// جزئیات مربوط به مهارت های خواسته شده در 
    /// </summary>
    public virtual ICollection<AdvertisementSkill> AdvertisementSkills { get; private set; } = new List<AdvertisementSkill>();

    /// <summary>
    /// جزئیات مربوط به درخواست های کاری این اگهی
    /// </summary>
    public virtual ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();

    /// <summary>
    /// جزئیات مربوط به پرداخت های یک اگهی
    /// </summary>
    public virtual ICollection<Payment> Payments { get; private set; } = new List<Payment>();

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new DomainException(DomainErrors.DescriptionIsRequired);

        if (Description.Length < 100 || Description.Length > 2_000)
            throw new DomainException(DomainErrors.DescriptionInvalidLength);

        if (MinimumAge < 18 || MinimumAge > 55)
            throw new DomainException(DomainErrors.MinimumAgeOutOfRange);

        if (MaximumAge < 18 || MaximumAge > 65)
            throw new DomainException(DomainErrors.MaximumAgeOutOfRange);

        if (MinimumAge > MaximumAge)
            throw new DomainException(DomainErrors.MinimumAgeCannotExceedMaximumAge);

        if (MaximumSalary < 1_000_000 || MaximumSalary > 600_000_000)
            throw new DomainException(DomainErrors.MaximumSalaryOutOfRange);

        if (ExperienceLevel < 0)
            throw new DomainException(DomainErrors.ExperienceLevelOutOfRange);

        if (JobId == Guid.Empty)
            throw new DomainException(DomainErrors.AdvertisementJobIdIsRequired);

        if (CityId == Guid.Empty)
            throw new DomainException(DomainErrors.AdvertisementCityIdIsRequired);

        if (CompanyId == Guid.Empty)
            throw new DomainException(DomainErrors.AdvertisementCompanyIdIsRequired);
    }

    /// <summary>
    /// اپدیت وضعیت فعال یا غیرذفعال بودن اگهی
    /// </summary>
    /// <param name="modifierId"></param>
    /// <param name="isActive"></param>
    public void UpdateActiveStatus(Guid? modifierId, bool isActive)
    {
        IsActive = isActive;

        Update(modifierId);
    }

    public void UpdateAdvertisementInfo(UpdateAdvertisementInfo updateAdvertisement)
    {
        if (updateAdvertisement.Description is not null)
            Description = updateAdvertisement.Description;

        if (updateAdvertisement.MinimumAge is not null)
            MinimumAge = updateAdvertisement.MinimumAge.Value;

        if (updateAdvertisement.MaximumAge is not null)
            MaximumAge = updateAdvertisement.MaximumAge.Value;

        if (updateAdvertisement.MinimumSalary is not null)
            MinimumSalary = updateAdvertisement.MinimumSalary.Value;

        if (updateAdvertisement.MaximumSalary is not null)
            MaximumSalary = updateAdvertisement.MaximumSalary.Value;

        if (updateAdvertisement.ExperienceLevel is not null)
            ExperienceLevel = updateAdvertisement.ExperienceLevel.Value;

        if (updateAdvertisement.CollaborationType is not null)
            CollaborationType = updateAdvertisement.CollaborationType.Value;

        Update(updateAdvertisement.ModifiedById);

        Validate();
    }
}
