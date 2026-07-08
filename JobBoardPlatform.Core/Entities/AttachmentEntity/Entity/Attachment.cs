using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;

namespace JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;

/// <summary>
/// پیوست فایل ها
/// </summary>
public class Attachment : BaseEntity
{
    private Attachment() { }

    public Attachment(string fileName, string contentType, byte[] data)
    {
        FileName = fileName;
        ContentType = contentType;
        Data = data;
    }

    /// <summary>
    /// اسم فایل اپلود شده 
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// نوع محتوا فایل اپدیت شده
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// دیتا فایل اپلود شده 
    /// </summary>
    public byte[] Data { get; set; }

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(FileName))
            throw new DomainException(DomainErrors.AttachmentFileNameIsRequired);

        if (string.IsNullOrWhiteSpace(ContentType))
            throw new DomainException(DomainErrors.AttachmentContentTypeIsRequired);
    }
}
