using JobBoardPlatform.Core.Entities.PaymentEntity.Dto;
using JobBoardPlatform.Core.Entities.PaymentEntity.Enums;
using System.Text.Json.Serialization;

namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.PaymentDto;

public class PaymentResponseDto
{
    public Guid PaymentId { get; set; }

    public decimal Amount { get; set; }

    public int DurationInDays { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PaymentStatus Status { get; set; }

    public Guid AdvertisementId { get; set; }

    public string AdvertisementTitle { get; set; } = string.Empty;

    public string CompanyName { get; set; } = string.Empty;

    public static PaymentResponseDto MapToResponseDto(PaymentDetail paymentDetail)
    {
        return new PaymentResponseDto
        {
            PaymentId = paymentDetail.PaymentId,
            Amount = paymentDetail.Amount,
            DurationInDays = paymentDetail.DurationInDays,
            Status = paymentDetail.Status,
            AdvertisementId = paymentDetail.AdvertisementId,
            AdvertisementTitle = paymentDetail.AdvertisementTitle,
            CompanyName = paymentDetail.CompanyName
        };
    }
}
