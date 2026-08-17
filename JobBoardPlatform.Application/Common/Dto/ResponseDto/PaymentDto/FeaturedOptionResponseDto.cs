namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.PaymentDto;

/// <summary>
/// گزینه های خرید اگهی ویژه مدت زمان و قیمت محاسبه شده سمت سرور
/// </summary>
public class FeaturedOptionResponseDto
{
    public int DurationInDays { get; set; }

    public decimal Price { get; set; }
}
