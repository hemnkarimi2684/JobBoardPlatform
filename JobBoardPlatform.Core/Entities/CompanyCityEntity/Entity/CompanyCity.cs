using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Entity;

namespace JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;

public class CompanyCity : BaseEntity
{
    private CompanyCity() { }

    public CompanyCity(string location, Guid companyId, Guid cityId, Guid? createdById = null)
    {
        Location = location;
        CompanyId = companyId;
        CityId = cityId;
        CreatedById = createdById;

        Validate(); 
    }

    /// <summary>
    /// ادرس مربوط به شرکت
    /// </summary>
    public string Location { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به شرکت
    /// </summary>
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// شناسه مربوط به شهری که شرکت در ان قرار دارد 
    /// </summary>
    public Guid CityId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به شرکت 
    /// </summary>
    public virtual Company Company { get; private set; }

    /// <summary>
    /// جزئیات مربوط به شهری که شرکت در ان قرار دارد 
    /// </summary>
    public virtual City City { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Location))
            throw new DomainException(DomainErrors.CompanyCityLocationIsRequired);

        if (Location.Length < 2 || Location.Length > 200)
            throw new DomainException(DomainErrors.CompanyCityLocationInvalidLength);
    }
}
