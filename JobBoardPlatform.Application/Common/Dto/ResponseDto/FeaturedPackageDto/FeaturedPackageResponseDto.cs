namespace JobBoardPlatform.Application.Common.Dto.ResponseDto.FeaturedPackageDto;

/// <summary>
/// بسته ویژه اگهی برای نمایش به ادمین (قیمت قابل ویرایش)
/// </summary>
public class FeaturedPackageResponseDto
{
    public Guid PackageId { get; set; }

    public int DurationInDays { get; set; }

    public decimal Price { get; set; }
}
