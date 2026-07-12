using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;

namespace JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;

/// <summary>
/// استان
/// </summary>
public class Province : BaseEntity
{
    private Province() { }

    public Province(string name, int provinceCode, Guid? createdById = null)
    {
        Name = name;
        ProvinceCode = provinceCode;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// نام استان
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// کد استان 
    /// </summary>
    public int ProvinceCode { get; private set; }

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به شهر های استان
    /// </summary>
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.ProvinceNameIsRequired);

        if (Name.Length < 2 || Name.Length > 100)
            throw new DomainException(DomainErrors.ProvinceNameInvalidLength);

        if (ProvinceCode < 1)
            throw new DomainException(DomainErrors.ProvinceCodeInvalidRange);
    }
}
