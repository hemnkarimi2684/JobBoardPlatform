using JobBoardPlatform.Core.Entities.Common.Data;
using JobBoardPlatform.Core.Entities.PaymentEntity.Dto;
using JobBoardPlatform.Core.Entities.PaymentEntity.Entity;

namespace JobBoardPlatform.Core.Entities.PaymentEntity.Data;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    /// <summary>
    /// دریافت جزئیات پرداخت همراه با اطلاعات اگهی و مدت زمان ویژه بودن
    /// </summary>
    /// <param name="paymentId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PaymentDetail?> GetPaymentDetailAsync(Guid paymentId, CancellationToken cancellationToken);
}
