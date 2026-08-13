using JobBoardPlatform.Application.Common.Dto.ResponseDto.FeaturedPackageDto;

namespace JobBoardPlatform.Application.Interfaces.FeaturedPackageInterface;

public interface IFeaturedPackageService
{
    /// <summary>
    /// دریافت تمام بسته های ویژه اگهی برای صفحه ادمین
    /// </summary>
    Task<List<FeaturedPackageResponseDto>> GetFeaturedPackagesAsync(
        CancellationToken cancellationToken);

    /// <summary>
    /// تغییر قیمت یک بسته ویژه
    /// </summary>
    Task UpdateFeaturedPackagePriceAsync(
        Guid packageId,
        decimal price,
        CancellationToken cancellationToken);
}
