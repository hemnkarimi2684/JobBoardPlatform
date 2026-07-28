using JobBoardPlatform.Application.Common.EmailSettings;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace JobBoardPlatform.Application.Implementation.EmailBusiness;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> options, ILogger<EmailService> logger)
    {
        _smtpSettings = options.Value;
        _logger = logger;
    }

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        bool isHtml,
        CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));

        message.To.Add(MailboxAddress.Parse(to));

        message.Subject = subject;

        message.Date = DateTimeOffset.UtcNow;

        message.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
        {
            Text = body
        };

        using var client = new MailKit.Net.Smtp.SmtpClient();

        var secureOption = _smtpSettings.UseSsl
            ? SecureSocketOptions.SslOnConnect
            : SecureSocketOptions.StartTls;

        try
        {
            _logger.LogInformation("Connecting to SMTP server {Server}:{Port}", _smtpSettings.Server, _smtpSettings.Port);

            await client.ConnectAsync(
                _smtpSettings.Server,
                _smtpSettings.Port,
                secureOption,
                cancellationToken);

            await client.AuthenticateAsync(
                _smtpSettings.UserName,
                _smtpSettings.Password,
                cancellationToken);

            await client.SendAsync(message, cancellationToken);

            _logger.LogInformation("Email sent successfully to {To} with subject {Subject}", to, subject);
        }
        catch (AuthenticationException ex)
        {
            //این اکسپشن برا وقتیه که توی لاگین به smtp به مشکل بخوره

            _logger.LogError(ex, "SMTP authentication failed");
            throw new EmailSendingException("SMTP authentication failed.", ex);
        }
        catch (MailKit.Net.Smtp.SmtpCommandException ex)
        {
            // این اکسپشن برای وقتیه که SMTP server یک دستور رو رد کنه یعنی چی
            //مثلا گیرنده نامعتبره یا سرور اجازه ارسال نمیده .....

            _logger.LogError(ex, "SMTP command failed");
            throw new EmailSendingException("SMTP command failed.", ex);
        }
        catch (MailKit.Net.Smtp.SmtpProtocolException ex)
        {
            //این خطا بیشتر برای مشکل پروتکل است یعنی چی
            // مثلا پاسخ غیرمنتظره از سرور
            // مشکل TSL/SSL ......

            _logger.LogError(ex, "SMTP protocol error");
            throw new EmailSendingException("SMTP protocol error.", ex);
        }
        catch (FormatException ex)
        {
            //این اگه ایمیل مقصد اشتباه باشه به این مشکل میخوره

            _logger.LogError(ex, "Invalid email address format");
            throw new EmailSendingException("Invalid email address format.", ex);
        }
        catch (OperationCanceledException ex)
        {
            // اینم همین طور که میدونی برا وقتیه که کنسل میشه

            _logger.LogWarning(ex, "Email sending was canceled");
            throw;
        }
        catch (Exception ex)
        {
            // اینم یه ترکیب دفاعی با ترکیب 12 0 هستش 

            _logger.LogError(ex, "Unexpected error while sending email");
            throw new EmailSendingException("Unexpected error while sending email.", ex);
        }
        finally
        {
            // اینم قسم خورده تا وقتی کانکشن رو نکشه از برق ول نمیکنه 

            if (client.IsConnected)
            {
                try
                {
                    await client.DisconnectAsync(true, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to disconnect SMTP client cleanly");
                }
            }
        }
    }
}
