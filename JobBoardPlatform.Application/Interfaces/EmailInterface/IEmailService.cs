using JobBoardPlatform.Application.Common.Dto.RequestDto.Common;
using JobBoardPlatform.Application.Common.Dto.RequestDto.EmailTemplateDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.EmailTemplateDto;
using JobBoardPlatform.Core.Entities.Common.Dto;

namespace JobBoardPlatform.Application.Interfaces.EmailInterface;

public interface IEmailService
{
    /// <summary>
    /// فرستادن پیام مورد نظر با قالب های موجود برای ایمیل
    /// </summary>
    /// <param name="templateKey"></param>
    /// <param name="to"></param>
    /// <param name="placeholders"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SendTemplateEmailAsync(
            string templateKey,
            string to,
            Dictionary<string, string>? placeholders = null,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// غیر فعال کردن قالب ایمیل 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isActive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeactivateTemplateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///  فعال کردن قالب ایمیل 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isActive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ActivateTemplateAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// اپدیت قالب ایمیل
    /// </summary>
    /// <param name="id"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateTemplateAsync(
        Guid id,
        UpdateTemplateRequestDto updateTemplateRequestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت تمام قالب های ایمیل
    /// </summary>
    /// <param name="pagingCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Pagination<EmailTemplateResponseDto>> GetAllAsync(
        PagingRequestDto pagingCommand,
        CancellationToken cancellationToken = default);
}
