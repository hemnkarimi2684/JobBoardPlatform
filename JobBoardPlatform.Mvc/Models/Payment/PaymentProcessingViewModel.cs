using JobBoardPlatform.Application.Common.Dto.ResponseDto.PaymentDto;
using JobBoardPlatform.Core.Entities.PaymentEntity.Enums;

namespace JobBoardPlatform.Mvc.Models.Payment;

public class PaymentProcessingViewModel
{
    public Guid PaymentId { get; set; }

    public Guid AdvertisementId { get; set; }

    public decimal Amount { get; set; }

    public int DurationInDays { get; set; }

    public string AdvertisementTitle { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; }

    public bool IsPending => Status == PaymentStatus.Pending;

    public static PaymentProcessingViewModel FromResponseDto(PaymentResponseDto source)
        => new()
        {
            PaymentId = source.PaymentId,
            AdvertisementId = source.AdvertisementId,
            Amount = source.Amount,
            DurationInDays = source.DurationInDays,
            AdvertisementTitle = source.AdvertisementTitle,
            CompanyName = source.CompanyName,
            Status = source.Status
        };
}
