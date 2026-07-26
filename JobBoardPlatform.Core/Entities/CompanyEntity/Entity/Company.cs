using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.CompanyCityEntity.Entity;
using JobBoardPlatform.Core.Entities.CompanyEntity.Dto;
using JobBoardPlatform.Core.Entities.CompanyEntity.Enums;
using JobBoardPlatform.Core.Entities.JobCategoryEntity.Entity;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.CompanyEntity.Entity;

/// <summary>
/// شرکت
/// </summary>
public class Company : BaseEntity
{
    private Company() { }

    public Company(string name, DateTime yearOfEstablishment, string aboutUs, string webSiteAddress, OwnershipType ownershipType, Guid ownedByUserId, CompanySizeEnum companySize, Guid jobCategoryId, string? activityType = null, Guid? companyImageFileId = null, Guid? createdById = null)
    {
        Name = name;
        YearOfEstablishment = yearOfEstablishment;
        AboutUs = aboutUs;
        WebSiteAddress = webSiteAddress;
        OwnershipType = ownershipType;
        OwnedByUserId = ownedByUserId;
        CompanySize = companySize;
        JobCategoryId = jobCategoryId;
        ActivityType = activityType;
        CompanyImageFileId = companyImageFileId;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// نام شرکت 
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// سال تاسیس شرکت 
    /// </summary>
    public DateTime YearOfEstablishment { get; private set; }

    /// <summary>
    /// درباره شرکت
    /// </summary>
    public string AboutUs { get; private set; }

    /// <summary>
    /// ادرس سایت شرکت
    /// </summary>
    public string WebSiteAddress { get; private set; }

    /// <summary>
    /// نوع مالکیت شرکت
    /// </summary>
    public OwnershipType OwnershipType { get; private set; }

    /// <summary>
    /// نعداد نفر کارکنان در شرکت
    /// </summary>
    public CompanySizeEnum CompanySize { get; private set; }

    /// <summary>
    /// نوع فعالیت شرکت
    /// </summary>
    public string? ActivityType { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه کارفرما شرکت
    /// </summary>
    public Guid OwnedByUserId { get; private set; }

    /// <summary>
    /// شناسه فایل تصویر شرکت
    /// </summary>
    public Guid? CompanyImageFileId { get; private set; }

    /// <summary>
    ///  زمینه فعالیت شرکت
    /// </summary>
    public Guid JobCategoryId { get; private set; }

    #endregion

    #region Navigation properties

    /// <summary>
    /// جزئیات مربوط به شهر هایی که شرکت در ان حضور داره 
    /// </summary>
    public virtual ICollection<CompanyCity> CompanyCities { get; private set; } = new List<CompanyCity>();

    /// <summary>
    /// جزئیات مربوط به اگهی های شرکت 
    /// </summary>
    public virtual ICollection<Advertisement> Advertisements { get; private set; } = new List<Advertisement>();

    /// <summary>
    /// جزئیات مربوط به کارفرما شرکت
    /// </summary>
    public virtual User OwnedByUser { get; private set; }

    /// <summary>
    /// جزئیات مربوط عکس اپلود شده شرکت
    /// </summary>
    public virtual Attachment? CompanyImageFile { get; private set; }

    /// <summary>
    /// جزئیات مربوط به زمینه فعالیت شرکت
    /// </summary>
    public virtual JobCategory JobCategory { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new DomainException(DomainErrors.CompanyNameIsRequired);

        if (Name.Length < 2 || Name.Length > 120)
            throw new DomainException(DomainErrors.CompanyNameInvalidLength);

        if (YearOfEstablishment > DateTime.UtcNow)
            throw new DomainException(DomainErrors.CompanyYearOfEstablishmentInvalidRange);

        if (string.IsNullOrWhiteSpace(AboutUs))
            throw new DomainException(DomainErrors.CompanyAboutUsIsRequired);

        if (AboutUs.Length < 50 || AboutUs.Length > 1_500)
            throw new DomainException(DomainErrors.CompanyAboutUsInvalidLength);

        if (string.IsNullOrWhiteSpace(WebSiteAddress))
            throw new DomainException(DomainErrors.CompanyWebSiteAddressIsRequired);

        if (WebSiteAddress.Length < 2 || WebSiteAddress.Length > 100)
            throw new DomainException(DomainErrors.CompanyWebSiteAddressInvalidLength);

        if (!string.IsNullOrWhiteSpace(ActivityType) && ActivityType?.Length > 120 || ActivityType?.Length < 0)
            throw new DomainException(DomainErrors.CompanyActivityTypeInvalidLength);

        if (OwnedByUserId == Guid.Empty)
            throw new DomainException(DomainErrors.CompanyOwnedByUserIdIsRequired);
    }

    public void UpdateCompanyInfo(CompanyInfoUpdate companyInfoUpdate)
    {
        if (companyInfoUpdate.Name is not null)
            Name = companyInfoUpdate.Name;

        if (companyInfoUpdate.YearOfEstablishment is not null)
            YearOfEstablishment = companyInfoUpdate.YearOfEstablishment.Value;

        if (companyInfoUpdate.JobCategoryId is not null)
            JobCategoryId = companyInfoUpdate.JobCategoryId.Value;

        if (companyInfoUpdate.AboutUs is not null)
            AboutUs = companyInfoUpdate.AboutUs;

        if (companyInfoUpdate.WebSiteAddress is not null)
            WebSiteAddress = companyInfoUpdate.WebSiteAddress;

        if (companyInfoUpdate.OwnershipType is not null)
            OwnershipType = companyInfoUpdate.OwnershipType.Value;

        if (companyInfoUpdate.CompanySize is not null)
            CompanySize = companyInfoUpdate.CompanySize.Value;

        if (companyInfoUpdate.ActivityType is not null)
            ActivityType = companyInfoUpdate.ActivityType;

        Update(companyInfoUpdate.ModifiedById);

        Validate();
    }

    public void UpdateImage(Guid? newImageId)
    {
        CompanyImageFileId = newImageId;
    }

}
