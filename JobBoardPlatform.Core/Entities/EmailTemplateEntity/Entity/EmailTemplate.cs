using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;

namespace JobBoardPlatform.Core.Entities.EmailTemplateEntity.Entity;

/// <summary>
/// قالب ایمیل
/// </summary>
public class EmailTemplate : BaseEntity
{
    private EmailTemplate() { }

    public EmailTemplate(string key, string subject, string body, Guid? createdById = null)
    {
        Key = key;
        Subject = subject;
        Body = body;
        IsActive = true;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// کلید قالب ایمیل
    /// </summary>
    public string Key { get; private set; }

    /// <summary>
    /// موضوع قالب ایمیل 
    /// </summary>
    public string Subject { get; private set; }

    /// <summary>
    /// متن قالب ایمیل
    /// </summary>
    public string Body { get; private set; }

    /// <summary>
    /// ایا فعاله یا نه 
    /// </summary>
    public bool IsActive { get; private set; }

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Key))
            throw new DomainException(DomainErrors.EmailKeyIsRequired);

        if (Key.Length < 3 || Key.Length > 100)
            throw new DomainException(DomainErrors.EmailKeyInvalidLength);

        if (string.IsNullOrWhiteSpace(Subject))
            throw new DomainException(DomainErrors.EmailSubjectIsRequired);

        if (Subject.Length < 3 || Subject.Length > 255)
            throw new DomainException(DomainErrors.EmailSubjectInvalidLength);

        if (string.IsNullOrWhiteSpace(Body))
            throw new DomainException(DomainErrors.EmailBodyIsRequired);
    }

    public void UpdateTemplate(string subject, string body, Guid? modifiedById)
    {
        Subject = subject.Trim();
        Body = body;

        Validate();
        Update(modifiedById);
    }

    public void UpdateActiveStatus(bool isActive, Guid? modifiedById)
    {
        IsActive = isActive;

        Update(modifiedById);
    }
}
