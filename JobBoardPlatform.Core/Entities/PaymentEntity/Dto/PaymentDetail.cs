using JobBoardPlatform.Core.Entities.PaymentEntity.Enums;

namespace JobBoardPlatform.Core.Entities.PaymentEntity.Dto;

/// <summary>
/// جزئیات یک پرداخت به همراه اطلاعات اگهی و مدت زمان ویژه بودن
/// </summary>
public class PaymentDetail
{
    public decimal Amount { get; set; }

    public int DurationInDays { get; set; }

    public PaymentStatus Status { get; set; }

    public Guid PaymentId { get; set; }
    public Guid AdvertisementId { get; set; }
    public Guid UserId { get; set; }

    public string AdvertisementTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}
