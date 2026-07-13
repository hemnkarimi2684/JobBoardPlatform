using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;

public class JobApplication : BaseEntity
{
    private JobApplication() { }
    
    public JobApplication(JobApplicationStatus status, Guid resumeId, Guid advertisementId, Guid userId, Guid? createdById = null)
    {
        Status = status;
        ResumeId = resumeId;
        AdvertisementId = advertisementId;
        UserId = userId;
        CreatedById = createdById;
    }

    public JobApplicationStatus Status { get; set; }

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

    protected override void Validate() => throw new NotImplementedException();
    
}
