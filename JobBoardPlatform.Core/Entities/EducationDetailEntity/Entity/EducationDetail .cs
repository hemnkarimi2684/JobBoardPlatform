using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Dto;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Enums;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Dto;
using JobBoardPlatform.Core.Entities.EducationDetailEntity.Enums;
using JobBoardPlatform.Core.Entities.ProvinceEntity.Entity;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.EducationDetailEntity.Entity;

/// <summary>
/// جزئیات تحصیلات 
/// </summary>
public class EducationDetail : BaseEntity
{
    private EducationDetail() { }

    public EducationDetail(CertificateDegree certificateDegreeName, string major, string university, DateTime startDate, DateTime? completionDate, double? percentage, bool isCurrentlyStudying, Guid userId, Guid? createdById = null)
    {
        CertificateDegreeName = certificateDegreeName;
        Major = major;
        University = university;
        StartDate = startDate;
        CompletionDate = completionDate;
        Percentage = percentage;
        IsCurrentlyStudying = isCurrentlyStudying;
        UserId = userId;
        CreatedById = createdById;

        HandleCurrentlyStudyingStatus(isCurrentlyStudying);

        Validate();
    }

    /// <summary>
    /// مقطع تحصیلی 
    /// </summary>
    public CertificateDegree CertificateDegreeName { get; private set; }

    /// <summary>
    /// رشته تحصیلی
    /// </summary>
    public string Major { get; private set; }

    /// <summary>
    /// دانشگاه
    /// </summary>
    public string University { get; private set; }

    /// <summary>
    /// سال شروع تحصیل در دانشگاه
    /// </summary>
    public DateTime StartDate { get; private set; }

    /// <summary>
    /// سال پایان تحصیل در دانشگاه
    /// </summary>
    public DateTime? CompletionDate { get; private set; }

    /// <summary>
    /// معدل
    /// </summary>
    public double? Percentage { get; private set; }

    /// <summary>
    /// ایا هنوز در حال تحصیل است؟
    /// </summary>
    public bool IsCurrentlyStudying { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به دارای مدرک تحصیلی
    /// </summary>
    public Guid UserId { get; private set; }

    #endregion

    #region Navigation properties

    /// <summary>
    /// جزئیات مربوط به کاربر دارای مدرک تحصیلی
    /// </summary>
    public virtual User User { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (string.IsNullOrWhiteSpace(Major))
            throw new DomainException(DomainErrors.EducationDetailMajorIsRequired);

        if (Major.Length < 2 || Major.Length > 120)
            throw new DomainException(DomainErrors.EducationDetailMajorInvalidLength);

        if (string.IsNullOrWhiteSpace(University))
            throw new DomainException(DomainErrors.EducationDetailUniversityIsRequired);

        if (University.Length < 2 || University.Length > 100)
            throw new DomainException(DomainErrors.EducationDetailUniversityInvalidLength);

        if (StartDate > DateTime.UtcNow.AddYears(1))
            throw new DomainException(DomainErrors.EducationDetailUniversityStartDateTooFarInFuture);

        if (CompletionDate is not null)
        {
            if (CompletionDate <= StartDate.AddYears(1))
                throw new DomainException(DomainErrors.EducationDetailUniversityDurationTooShort);
        }

        if (Percentage is not null)
        {
            if (Percentage < 12)
                throw new DomainException(DomainErrors.EducationDetailFinalGradeTooLow);
        }

        if (UserId == Guid.Empty)
            throw new DomainException(DomainErrors.EducationDetailUserIdIsRequired);
    }

    /// <summary>
    /// هندل کردن وضعیت ایا درحال حاضر در حال تحصیل است یا نه؟
    /// </summary>
    /// <param name="isCurrentlyStudying"></param>
    private void HandleCurrentlyStudyingStatus(bool isCurrentlyStudying)
    {
        if (!isCurrentlyStudying)
            return;

        CompletionDate = null;
        Percentage = null;
    }

    public void UpdateEducationDetailInfo(UpdateEducationDetail updateEducation)
    {
        if (updateEducation.CertificateDegreeName is not null)
            CertificateDegreeName = updateEducation.CertificateDegreeName.Value;

        if (updateEducation.Major is not null)
            Major = updateEducation.Major;

        if (updateEducation.CertificateDegreeName is not null)
            CertificateDegreeName = updateEducation.CertificateDegreeName.Value;

        if (updateEducation.StartDate is not null)
            StartDate = updateEducation.StartDate.Value;

        if (updateEducation.CompletionDate is not null)
            CompletionDate = updateEducation.CompletionDate;

        if (updateEducation.Percentage is not null)
            Percentage = updateEducation.Percentage;

        Update(updateEducation.ModifiedById);

        Validate();
    }
}
