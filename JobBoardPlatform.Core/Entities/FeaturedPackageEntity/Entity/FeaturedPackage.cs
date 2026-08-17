using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.Common.Entity;

namespace JobBoardPlatform.Core.Entities.FeaturedPackageEntity.Entity;

/// <summary>
/// بسته های ویژه اگهی مدت زمان و قیمت قابل تنظیم توسط ادمین
/// </summary>
public class FeaturedPackage : BaseEntity
{
    private FeaturedPackage() { }

    public FeaturedPackage(int durationInDays, decimal price, Guid? createdById = null)
    {
        DurationInDays = durationInDays;
        Price = price;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// تعداد روزهای ویژه بودن اگهی
    /// </summary>
    public int DurationInDays { get; private set; }

    /// <summary>
    /// قیمت این بسته به تومان
    /// </summary>
    public decimal Price { get; private set; }

    public void UpdatePrice(decimal price, Guid? modifiedById)
    {
        Price = price;

        Validate();

        Update(modifiedById);
    }

    protected override void Validate()
    {
        if (DurationInDays != 7 && DurationInDays != 15 && DurationInDays != 30)
            throw new DomainException(DomainErrors.FeaturedPackageDurationNotAllowed);

        if (Price <= 0)
            throw new DomainException(DomainErrors.FeaturedPackagePriceOutOfRange);
    }
}
