using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.ExperienceDetailEntity.Entity;

/// <summary>
/// سوابق شغلی
/// </summary>
public class ExperienceDetail : BaseEntity
{
    private ExperienceDetail() { }

    public ExperienceDetail(string lastJobTitle, SeniorityLevel seniorityLevel, string jobCategory, string city, DateTime startDate, DateTime? endDate, bool isCurrentJob, Guid userId, Guid? createdById = null)
    {
        HandleCurrentlyWorkingStatus(isCurrentJob);

        LastJobTitle = lastJobTitle;
        SeniorityLevel = seniorityLevel;
        JobCategory = jobCategory;
        City = city;
        StartDate = startDate;
        EndDate = endDate;
        IsCurrentJob = isCurrentJob;
        UserId = userId;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// اخرین عنوان شغلی
    /// </summary>
    public string LastJobTitle { get; private set; }

    /// <summary>
    /// رده سازمانی
    /// </summary>
    public SeniorityLevel SeniorityLevel { get; private set; }

    /// <summary>
    /// زمینه فعالیت کاربر توی شرکت
    /// </summary>
    public string JobCategory { get; private set; }

    /// <summary>
    /// شهری که کاربر در ان فعالیت داشته
    /// </summary>
    public string City { get; private set; }

    /// <summary>
    /// تاریخ شروع کار
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// تاریخ پایان کار
    /// </summary>
    public DateTime? EndDate { get; private set; }

    /// <summary>
    /// کاربر هنوز مشغول به این کار است یا نه؟
    /// </summary>
    public bool IsCurrentJob { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به کاربر دارای سابقه شغلی
    /// </summary>
    public Guid UserId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به کاربر دارای سابقه شغلی 
    /// </summary>
    public virtual User User { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(LastJobTitle))
            throw new DomainException(DomainErrors.ExperienceDetailLastJobTitleIsRequired);

        if (LastJobTitle.Length < 2 || LastJobTitle.Length > 120)
            throw new DomainException(DomainErrors.ExperienceDetailLastJobTitleInvalidLength);

        if (string.IsNullOrWhiteSpace(JobCategory))
            throw new DomainException(DomainErrors.ExperienceDetailJobCategoryIsRequired);

        if (JobCategory.Length < 2 || JobCategory.Length > 100)
            throw new DomainException(DomainErrors.ExperienceDetailJobCategoryInvalidLength);

        if (string.IsNullOrWhiteSpace(City))
            throw new DomainException(DomainErrors.ExperienceDetailCityIsRequired);

        if (City.Length < 2 || City.Length > 100)
            throw new DomainException(DomainErrors.ExperienceDetailCityInvalidLength);

        if (StartDate > DateTime.UtcNow.AddYears(2))
            throw new DomainException(DomainErrors.ExperienceDetailStartDateTooFarInFuture);

        if (EndDate is not null)
        {
            if (EndDate < StartDate)
                throw new DomainException(DomainErrors.ExperienceDetailJobEndTimeLowerThanStartTime);
        }
    }

    /// <summary>
    /// هندل کردن وضعیت اینکه کاربر در حال کار است یا نه؟
    /// </summary>
    /// <param name="isCurrentJob"></param>
    private void HandleCurrentlyWorkingStatus(bool isCurrentJob)
    {
        if (!IsCurrentJob)
            return;

        EndDate = null;
    }

    public void UpdateExperienceDetailInfo(UpdateExperienceDetail updateExperienceDetail)
    {
        if (updateExperienceDetail.LastJobTitle is not null)
            LastJobTitle = updateExperienceDetail.LastJobTitle;

        if (updateExperienceDetail.SeniorityLevel is not null)
            SeniorityLevel = updateExperienceDetail.SeniorityLevel.Value;

        if (updateExperienceDetail.JobCategory is not null)
            JobCategory = updateExperienceDetail.JobCategory;

        if (updateExperienceDetail.City is not null)
            City = updateExperienceDetail.City;

        if (updateExperienceDetail.StartDate is not null)
            StartDate = updateExperienceDetail.StartDate.Value;

        if (updateExperienceDetail.EndDate is not null)
            EndDate = updateExperienceDetail.EndDate;

        if (updateExperienceDetail.IsCurrentJob is not null)
            IsCurrentJob = updateExperienceDetail.IsCurrentJob.Value;

        Update(updateExperienceDetail.ModifiedById);

        Validate();
    }
}
