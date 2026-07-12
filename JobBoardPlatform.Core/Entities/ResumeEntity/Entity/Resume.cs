using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.JobApplicationEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.ResumeEntity.Entity;

/// <summary>
/// رزومه
/// </summary>
public class Resume : BaseEntity
{
    private Resume() { }
    
    public Resume(string title, Guid userId, Guid? lastUploadedFileId = null, Guid? createdById = null)
    {
        Title = title;
        UserId = userId;
        LastUploadedFileId = lastUploadedFileId;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// عنوان رزومه
    /// </summary>
    public string Title { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به کاربر دارای رزومه
    /// </summary>
    public virtual Guid UserId { get; private set; }

    /// <summary>
    /// فایل اپلود شده رزومه
    /// </summary>
    public Guid? LastUploadedFileId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط کاربری که رزومه دارد
    /// </summary>
    public virtual User User { get; private set; } 

    /// <summary>
    /// جزئیات مربوط به اخرین فایل اپلود شده رزومه 
    /// </summary>
    public virtual Attachment? LastUploadedFile { get; private set; } 

    /// <summary>
    /// جزئیات مربوط به درخواست هایی که با این رزومه داده شده
    /// </summary>
    public virtual ICollection<JobApplication> JobApplications { get; private set; } = new List<JobApplication>();

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new DomainException(DomainErrors.ResumeTitleIsRequired);

        if (Title.Length < 2 || Title.Length > 100)
            throw new DomainException(DomainErrors.ResumeTitleInvalidLength);
    }
}
