using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.ResumeEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using System.Net.NetworkInformation;

namespace JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;

public class JobApplication : BaseEntity
{
    private JobApplication() { }
    
    public JobApplication(Guid statusId, Guid resumeId, Guid advertisementId, Guid userId)
    {
        StatusId = statusId;
        ResumeId = resumeId;
        AdvertisementId = advertisementId;
        UserId = userId;
    }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به وضعیت درخواست کار
    /// </summary>
    public Guid StatusId { get; private set; }

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
    /// جزئیات مربوط به وضعیت درخواست کار
    /// </summary>
    public virtual Status Status { get; private set; }

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
