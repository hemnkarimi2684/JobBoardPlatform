using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;

namespace JobBoardPlatform.Core.Entities.CityEntity.Entity;

/// <summary>
/// شهر
/// </summary>
public class City : BaseEntity
{
    private City() { }

    public City(string name, int cityCode, int provinceCode, Guid provinceId, Guid? createdById = null)
    {
        Name = name;
        CityCode = cityCode;
        ProvinceCode = provinceCode;
        ProvinceId = provinceId;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// اسم شهر
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// کد شهر
    /// </summary>
    public int CityCode { get; private set; }

    /// <summary>
    /// کد استان
    /// </summary>
    public int ProvinceCode { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به استان شهر 
    /// </summary>
    public Guid ProvinceId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به استان شهر
    /// </summary>
    public virtual Province Province { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.CityNameIsRequired);

        if (Name.Length < 2 || Name.Length > 100)
            throw new DomainException(DomainErrors.CityNameInvalidLength);

        if (CityCode < 1)
            throw new DomainException(DomainErrors.CityCodeInvalidRange);

        if (ProvinceCode < 1)
            throw new DomainException(DomainErrors.CityProvinceCodeInvalidRange);
    }
}
