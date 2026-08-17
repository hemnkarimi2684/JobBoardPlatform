using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.EmailTemplateEntity.Entity;

namespace JobBoardPlatform.Core.Entities.EmailTemplateEntity.Data;

public interface IEmailTemplateRepository : IGenericRepository<EmailTemplate>
{
    /// <summary>
    /// دریافت قالب ایمیل توسط کلیدش
    /// </summary>
    /// <param name="templateKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<EmailTemplate?> GetByKeyAsync(
        string templateKey,
        CancellationToken cancellationToken);
}
