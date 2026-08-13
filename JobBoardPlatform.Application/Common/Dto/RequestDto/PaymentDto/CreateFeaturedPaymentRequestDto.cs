using System.ComponentModel.DataAnnotations;

namespace JobBoardPlatform.Application.Common.Dto.RequestDto.PaymentDto;

public class CreateFeaturedPaymentRequestDto
{
    [Required(ErrorMessage = "identifier is required.")]
    [RegularExpression(@"^(?!00000000-0000-0000-0000-000000000000$).*$", ErrorMessage = "Invalid identifier.")]
    public Guid AdvertisementId { get; set; }

    [Required(ErrorMessage = "Featured duration is required.")]
    [Range(1, 30, ErrorMessage = "Featured duration must be between 1 and 30 days.")]
    public int DurationInDays { get; set; }
}
