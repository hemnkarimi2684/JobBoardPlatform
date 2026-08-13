using JobBoardPlatform.Application.Common.Dto.ResponseDto.PaymentDto;
using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Mvc.Models.Payment;

public class FeaturedPaymentViewModel
{
    [Required(ErrorMessage = "Advertisement is required.")]
    public Guid AdvertisementId { get; set; }

    [Required(ErrorMessage = "Please select a featured duration.")]
    [Range(1, 30, ErrorMessage = "Featured duration must be between 1 and 30 days.")]
    public int DurationInDays { get; set; }

    public List<FeaturedOptionResponseDto> Options { get; set; } = new();

    public static FeaturedPaymentViewModel FromResponseDto(Guid advertisementId, List<FeaturedOptionResponseDto> options)
        => new()
        {
            AdvertisementId = advertisementId,
            Options = options
        };
}
