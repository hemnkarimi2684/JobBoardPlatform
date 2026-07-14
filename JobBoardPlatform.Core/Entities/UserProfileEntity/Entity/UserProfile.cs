using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AttachmentEntity.Entity;
using JobBoardPlatform.Core.Entities.CityEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Dto;
using JobBoardPlatform.Core.Entities.UserProfileEntity.Enums;

namespace JobBoardPlatform.Core.Entities.UserProfileEntity.Entity;

/// <summary>
/// اطلاعات پروفایل کاربر
/// </summary>
public class UserProfile : BaseEntity
{
    private UserProfile() { }

    public UserProfile(string firstName, string lastName, string bio, string address, DateTime birthDate, Guid userId, Guid cityId, Gender gender, Guid? userImageFileId = null, Guid? createdById = null)
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
        CreatedById = createdById;

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

    public void UpdateUserInfo(UpdateUserProfile updateUserProfile)
    {
        if (updateUserProfile.FirstName is not null)
            FirstName = updateUserProfile.FirstName;

        if (updateUserProfile.LastName is not null)
            LastName = updateUserProfile.LastName;

        if (updateUserProfile.Bio is not null)
            Bio = updateUserProfile.Bio;

        if (updateUserProfile.Address is not null)
            Address = updateUserProfile.Address;

        if (updateUserProfile.BirthDate is not null)
            BirthDate = updateUserProfile.BirthDate.Value;

        if (updateUserProfile.Gender is not null)
            Gender = updateUserProfile.Gender.Value;

        if (updateUserProfile.CityId is not null)
            CityId = updateUserProfile.CityId.Value;

        Update(updateUserProfile.ModifiedById);

        Validate();
    }
}
