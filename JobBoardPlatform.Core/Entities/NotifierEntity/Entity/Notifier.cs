using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.NotifierEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.NotifierEntity.Entity;

/// <summary>
/// برای اطلاع رسانی 
/// </summary>
public class Notifier : BaseEntity
{
    private Notifier() { }

    public Notifier(string title, string message, NoticeType noticeType, Guid recipientUserId)
    {
        Title = title;
        Message = message;
        NoticeType = noticeType;
        IsRead = false;
        RecipientUserId = recipientUserId;

        Validate();
    }

    /// <summary>
    /// عنوان اعلان
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// پیام اعلان
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// نوع اعلان
    /// </summary>
    public NoticeType NoticeType { get; private set; }

    /// <summary>
    /// ایا خونده شده یا نه 
    /// </summary>
    public bool IsRead { get; private set; }

    /// <summary>
    /// در چه زمانی خونده شده
    /// </summary>
    public DateTime? ReadAt { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// این اعلان به کدوم شناسه کاربر رفته؟
    /// </summary>
    public Guid RecipientUserId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به کاربری که این اعلان رو داره
    /// </summary>
    public virtual User RecipientUser { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new DomainException(DomainErrors.NotifierTitleIsRequired);

        if (Title.Length < 2 || Title.Length > 150)
            throw new DomainException(DomainErrors.NotifierTitleInvalidLength);

        if (string.IsNullOrWhiteSpace(Message))
            throw new DomainException(DomainErrors.NotifierMessageIsRequired);

        if (Message.Length < 10 || Message.Length > 250)
            throw new DomainException(DomainErrors.NotifierMessageInvalidLength);

        if (RecipientUserId == Guid.Empty)
            throw new DomainException(DomainErrors.NotifierRecipientUserIdIsRequired);
    }

    public void Read(Guid modifiedById)
    {
        if (IsRead)
            return;

        IsRead = true;
        ReadAt = DateTime.UtcNow;

        Update(modifiedById);
    }
}
