using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Data;
using System.Linq.Expressions;

namespace JobBoardPlatform.Core.Entities.AttachmentEntity.Data;

public interface IAttachmentRepository : IGenericRepository<Attachment>
{

    /// <summary>
    /// دریافت فایل ذخیره شه توسط شناسه اش
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="projection"></param>
    /// <param name="attachmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult?> GetAttachmentByIdAsync<TResult>(
        Expression<Func<Attachment, TResult>> projection,
        Guid attachmentId,
        CancellationToken cancellationToken);

    /// <summary>
    /// حذف کامل فایل ذخیره شده
    /// </summary>
    /// <param name="attachmentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> HardDeleteAttachmentAsync(
        Guid attachmentId,
        CancellationToken cancellationToken);
}
