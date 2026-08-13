using JobBoardPlatform.Application.Common.Dto.RequestDto.PaymentDto;
using JobBoardPlatform.Application.Common.Dto.ResponseDto.PaymentDto;

namespace JobBoardPlatform.Application.Interfaces.PaymentInterface;

public interface IPaymentService
{
    /// <summary>
    /// ایجاد درگاه پرداخت برای ویژه کردن اگهی
    /// </summary>
    /// <param name="createCommand"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> CreateFeaturedPaymentAsync(
        CreateFeaturedPaymentRequestDto createCommand,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت جزئیات یک پرداخت
    /// </summary>
    /// <param name="paymentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaymentResponseDto> GetPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// دریافت گزینه های خرید اگهی ویژه با قیمت محاسبه شده سمت سرور
    /// </summary>
    /// <returns></returns>
    List<FeaturedOptionResponseDto> GetFeaturedOptions();

    /// <summary>
    /// تایید موفقیت آمیز بودن پرداخت و ویژه کردن اگهی
    /// </summary>
    /// <param name="paymentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ConfirmSuccessfulPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// ثبت عدم موفقیت پرداخت
    /// </summary>
    /// <param name="paymentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ConfirmFailedPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// انصراف از پرداخت
    /// </summary>
    /// <param name="paymentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CancelPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken = default);
}
