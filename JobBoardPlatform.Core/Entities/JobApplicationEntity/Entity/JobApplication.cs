using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using System.Xml.Linq;

namespace JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;

public class JobApplication : BaseEntity
{
    private JobApplication() { }

    public JobApplication(JobApplicationStatus status, string jobTitle, string companyName, string cityName, CollaborationType collaborationType, string userFullName, int experienceLevel, Guid resumeId, Guid advertisementId, Guid userId, Guid? createdById)
    {
        Status = status;
        JobTitle = jobTitle;
        CompanyName = companyName;
        CityName = cityName;
        CollaborationType = collaborationType;
        UserFullName = userFullName;
        ExperienceLevel = experienceLevel;
        ResumeId = resumeId;
        AdvertisementId = advertisementId;
        UserId = userId;
        CreatedById = createdById;
        Validate();
    }



    /// <summary>
    /// وضعیت درخواست کاری
    /// </summary>
    public JobApplicationStatus Status { get; set; }

    /// <summary>
    /// عنوان کاری این درخواست
    /// </summary>
    public string JobTitle { get; private set; }

    /// <summary>
    /// اسم شرکت مربوط به درخواست کاری 
    /// </summary>
    public string CompanyName { get; private set; }

    /// <summary>
    /// اسم شهر مربوط به درخواست کاری
    /// </summary>
    public string CityName { get; private set; }

    /// <summary>
    /// نوع همکاری در کار
    /// </summary>
    public CollaborationType CollaborationType { get; private set; }

    /// <summary>
    /// اسم کاربر درخواست دهنده
    /// </summary>
    public string UserFullName { get; private set; }

    /// <summary>
    /// سال های تجربه کاری 
    /// </summary>
    public int ExperienceLevel { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به رزومه 
    /// </summary>
    public Guid ResumeId { get; private set; }

    /// <summary>
    /// شناسه مربوط به اگهی
    /// </summary>
    public Guid AdvertisementId { get; private set; }

    /// <summary>
    /// شناسه مربوط به کاربر
    /// </summary>
    public Guid UserId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به رزومه
    /// </summary>
    public virtual Resume Resume { get; private set; }

    /// <summary>
    /// جزئیات مربوط به اگهی
    /// </summary>
    public virtual Advertisement Advertisement { get; private set; }

    /// <summary>
    /// جزئیات مربوط به کاربر
    /// </summary>
    public virtual User User { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(JobTitle))
            throw new DomainException(DomainErrors.JobTitleIsRequired);

        if (JobTitle.Length < 2 || JobTitle.Length > 100)
            throw new DomainException(DomainErrors.JobTitleInvalidLength);

        if (string.IsNullOrWhiteSpace(CompanyName))
            throw new DomainException(DomainErrors.JobApplicationCompanyNameIsRequired);

        if (CompanyName.Length < 2 || CompanyName.Length > 120)
            throw new DomainException(DomainErrors.JobApplicationCompanyNameInvalidLength);

        if (string.IsNullOrWhiteSpace(CityName))
            throw new DomainException(DomainErrors.JobApplicationCityNameIsRequired);

        if (CityName.Length < 2 || CityName.Length > 100)
            throw new DomainException(DomainErrors.JobApplicationCityNameInvalidRange);

        if (string.IsNullOrWhiteSpace(UserFullName))
            throw new DomainException(DomainErrors.FullNameIsRequired);

        if (UserFullName.Length < 2 || UserFullName.Length > 100)
            throw new DomainException(DomainErrors.FullNameInvalidLength);

        if(ExperienceLevel < 0)
            throw new DomainException(DomainErrors.JobApplicationExperienceLevelOutOfRange);

        if (UserId == Guid.Empty)
            throw new DomainException(DomainErrors.JobApplicationUserIdIsRequired);

        if (ResumeId == Guid.Empty)
            throw new DomainException(DomainErrors.JobApplicationResumeIdIsRequired);

        if (AdvertisementId == Guid.Empty)
            throw new DomainException(DomainErrors.JobApplicationAdvertisementIdIsRequired);
    }

    public void UpdateStatus(JobApplicationStatus status, Guid? modifiedById)
    {
        Status = status;

        Update(modifiedById);
    }
}
