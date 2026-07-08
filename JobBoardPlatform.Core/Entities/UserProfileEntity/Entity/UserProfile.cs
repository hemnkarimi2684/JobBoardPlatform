using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Common.Extensions;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;
using System.Reflection;

namespace JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;

/// <summary>
/// اطلاعات پروفایل کاربر
/// </summary>
public class UserProfile : BaseEntity
{
    private UserProfile() { }

    public UserProfile(string firstName, string lastName, string bio, string address, DateTime birthDate, Guid userId, Guid cityId, Gender gender, Guid? userImageFileId = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Bio = bio;
        Address = address;
        BirthDate = birthDate;
        UserId = userId;
        CityId = cityId;
        Gender = gender;
        UserImageFileId = userImageFileId;

        Validate();
    }

    /// <summary>
    /// نام کاربر
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// نام  خانوادگی کاربر
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// بیوگرافی کاربر
    /// </summary>
    public string Bio { get; private set; }

    /// <summary>
    /// ادرس کاربر
    /// </summary>
    public string Address { get; private set; }

    /// <summary>
    /// تاریخ تولد کاربر
    /// </summary>
    public DateTime BirthDate { get; private set; }

    /// <summary>
    /// جنسیت کاربر
    /// </summary>
    public Gender Gender { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به کاربر 
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// شناسه مربوط به شهر کاربر 
    /// </summary>
    public Guid CityId { get; private set; }

    /// <summary>
    /// شناسه فایل تصویر کاربر
    /// </summary>
    public Guid? UserImageFileId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات اطلاعات سیستمی کاربر
    /// </summary>
    public virtual User User { get; private set; }

    /// <summary>
    /// جزئیات مربوط به شهر کاربر 
    /// </summary>
    public virtual City City { get; private set; }

    /// <summary>
    /// جزئیات مربوط به عکس پروفایل کاربر
    /// </summary>
    public virtual Attachment? UserImageFile { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new DomainException(DomainErrors.FirstNameIsRequired);

        if (FirstName.Length < 2 || FirstName.Length > 100)
            throw new DomainException(DomainErrors.FistNameInvalidLength);

        if (string.IsNullOrWhiteSpace(LastName))
            throw new DomainException(DomainErrors.LastNameIsRequired);

        if (LastName.Length < 2 || LastName.Length > 100)
            throw new DomainException(DomainErrors.LastNameInvalidLength);

        if (string.IsNullOrWhiteSpace(Bio))
            throw new DomainException(DomainErrors.BioIsRequired);

        if (Bio.Length < 5 || Bio.Length > 250)
            throw new DomainException(DomainErrors.BioInvalidLength);

        if (string.IsNullOrWhiteSpace(Address))
            throw new DomainException(DomainErrors.AddressIsRequired);

        if (Address.Length < 2 || Address.Length > 250)
            throw new DomainException(DomainErrors.AddressInvalidLength);

        if (BirthDate > DateTime.UtcNow.Date.AddYears(-18))
            throw new DomainException(DomainErrors.UserMustBeAtLeast18YearsOld);
    }
}
