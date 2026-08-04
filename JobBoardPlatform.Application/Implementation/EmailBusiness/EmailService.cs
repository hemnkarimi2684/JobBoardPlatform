using JobBoardPlatform.Application.Common.CurrentUser.Interface;
using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EmailTemplateDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EmailTemplateDto;
using JobBoardPlatform.Application.Common.EmailSettings;
using JobBoardPlatform.Application.Common.Exceptions.ApplicationExceptions;
using JobBoardPlatform.Application.Interfaces.AccessControlInterface;
using JobBoardPlatform.Application.Interfaces.EmailInterface;
using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.Common.Dto;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace JobBoardPlatform.Application.Implementation.EmailBusiness;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtpSettings;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUser _currentUser;

    private readonly IAccessControlService _accessControlService;

    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> options, IUnitOfWork unitOfWork, ICurrentUser currentUser, IAccessControlService accessControlService, ILogger<EmailService> logger)
    {
        _smtpSettings = options.Value;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
        _accessControlService = accessControlService;
        _logger = logger;
    }

    #region Update Methods

    public async Task ActivateTemplateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var template = await _unitOfWork.EmailTemplateRepository.GetByIdAsync(id, cancellationToken, true);

        if (template is null)
            throw new NotFoundException($"email template was not found.");

        if (template.IsActive)
            throw new ConflictException($"email template is already active.");

        template.UpdateActiveStatus(true, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeactivateTemplateAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var template = await _unitOfWork.EmailTemplateRepository.GetByIdAsync(id, cancellationToken, true);

        if (template is null)
            throw new NotFoundException($"email template was not found.");

        if (!template.IsActive)
            throw new ConflictException($"email template is already inactive.");

        template.UpdateActiveStatus(false, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTemplateAsync(
    Guid id,
    UpdateTemplateRequestDto updateTemplateRequestDto,
    CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        var template = await _unitOfWork.EmailTemplateRepository.GetByIdAsync(id, cancellationToken, true);

        if (template is null)
            throw new NotFoundException($"email template was not found.");

        if (template.Subject == updateTemplateRequestDto.Subject.Trim() && template.Body == updateTemplateRequestDto.Body)
            return;

        template.UpdateTemplate(updateTemplateRequestDto.Subject, updateTemplateRequestDto.Body, _currentUser.UserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Get Methods

    public async Task<Pagination<EmailTemplateResponseDto>> GetAllAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default)
    {
        _accessControlService.EnsureAdmin(_currentUser);

        return await _unitOfWork.EmailTemplateRepository.QueryAsync(et => new EmailTemplateResponseDto
        {
            Id = et.Id,
            IsActive = et.IsActive,
            Subject = et.Subject,
            Body = et.Body,
            Key = et.Key
        }
        , cancellationToken
        , pagingCommand.PageNumber,
        pagingCommand.PageSize);
    }

    #endregion

    #region Send Email Methods

    private async Task SendAsync(
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

    public async Task SendTemplateEmailAsync(
        string templateKey,
        string to,
        Dictionary<string, string>? placeholders = null,
        CancellationToken cancellationToken = default)
    {
        var template = await _unitOfWork.EmailTemplateRepository.GetByKeyAsync(templateKey, cancellationToken);

        // اینجا اگر قالب پیدا نشد لاگ میندازم که پیدا نشده
        if (template == null)
        {
            _logger.LogWarning("Email template with key '{Key}' was not found.", templateKey);
            return;
        }

        // اینجا هم اگر قالب غیر فعال بود لاگ میندازم میگم که این قالب برای استفاده غیر فعال شده 
        if (!template.IsActive)
        {
            _logger.LogInformation("Email template '{Key}' is inactive. Email to {To} was skipped.", templateKey, to);
            return;
        }

        // حالا اگر هیچکدوم مشکل نداشت متن و موضوع اون قالبی که پیدا شده رو میندازم توی متغیر که استفاده کنم
        var subject = template.Subject;
        var body = template.Body;

        // این palceHolder چیه؟
        // این برای موافعیه که توی متن موردنظرم من از مواردی مثل {UserName} یا {FullName} استفاده میکنم
        // خب اصلا اینا به چه دردی میخورن ؟ برای اینه که پیام من خوانا تر و واضح تر و حرفه ای تر باشه به عنوان مثال فرض کن قراره هر سال یه پیام ارسال کنی مثلا
        // سلام علی توادت مبارک یا سلام کریم تولد مبارک خب اگه من پلیس هلدر نداشته باشم مجبورم یا شخصی سازی انجام ندم مثلا فقط بنویسم تولدت مبارک 
        // یا اینکه یه پلیس هلدر مثل سلام {Name} تولدت مبارک بزارم که به ازازی هر نفر ضخصی سازی انجام بدم

        // حالا این فور ایچه سبکش اینه اون دیکشنری از مقادیری که اومده رو بیاد داخل متن و موضوع مون جاگذاری کنه به عنوان مثال 
        // فرض کن من یه قالب به این شکل دارم سلام {{UserName}} به شرکت {{CompanyName}}  خوش اومدید 
        //حالا برای اینکه بیام مقادیر داخل یوزر نیم و کمپانی نیم رو پر کنم از پلیس هلدر استفاده میکنم چجوری ؟
        //فرض کن دیکشنری تو ولیو کریم و کلید UserName 
        // و ولیو اریا و کلیو CompanyName اومده توی ورودی 
        // حالا برای اینکه بیای این مقادیر رو توی اون قالب بزاری
        // میای اون کلید هاشون رو به شکلی که توی قالب نگه داشتی نگه میداری 
        // بعدش میری توی متن و موضوع قالبه  میگردی هر کجا که این کلید رو پیدا کردیی اون رو با مقدار ولیو همون کلیده پر میکنی
        // مثلا میری توی موضوع ها دمیگردی با کلید {{UserName}} 
        // فرض کن پیداش میکنی حالا که پیداش کردی میای این کلید رو با مقدار ولیو توی دیکشنری جایگزین میکنی یعنی الان مقدار توی متن شده کریم

        if (placeholders is not null && placeholders.Count > 0)
        {
            foreach (var placeholder in placeholders)
            {
                var target = $"{{{{{placeholder.Key}}}}}";

                subject = subject.Replace(target, placeholder.Value);

                body = body.Replace(target, placeholder.Value);
            }
        }

        await SendAsync(to, subject, body, false, cancellationToken);
    }

    #endregion
}
