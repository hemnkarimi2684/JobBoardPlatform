namespace JobBoardPlatform.Application.Interfaces.EmailInterface;

public interface IEmailService
{
    /// <summary>
    /// فرستادن پیام مورد نظر برای ایمیل
    /// </summary>
    /// <param name="to"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="isHtml"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendAsync(
        string to,
        string subject,
        string body, 
        bool isHtml, 
        CancellationToken cancellationToken = default);
}
