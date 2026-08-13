using JobBoardPlatform.Core.Common.Exceptions.DomainExceptions;
using JobBoardPlatform.Core.Entities.AdvertisementEntity.Entity;
using JobBoardPlatform.Core.Entities.Common.Entity;
using JobBoardPlatform.Core.Entities.PaymentEntity.Enums;
using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.PaymentEntity.Entity;

/// <summary>
/// پرداخت مدل
/// </summary>
public class Payment : BaseEntity
{
    private Payment() { }

    public Payment(decimal amount, PaymentStatus status, Guid advertisementId, Guid userId, Guid? createdById = null)
    {
        Amount = amount;
        Status = status;
        AdvertisementId = advertisementId;
        UserId = userId;
        CreatedById = createdById;

        Validate();
    }

    public Payment(decimal amount, int durationInDays, PaymentStatus status, Guid advertisementId, Guid userId, Guid? createdById = null)
    {
        Amount = amount;
        DurationInDays = durationInDays;
        Status = status;
        AdvertisementId = advertisementId;
        UserId = userId;
        CreatedById = createdById;

        Validate();
    }

    /// <summary>
    /// مقداری که باید پرداخت شود 
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// تعداد روزهای ویژه بودن اگهی خریداری شده
    /// </summary>
    public int DurationInDays { get; private set; }

    /// <summary>
    /// وضعیت پرداخت 
    /// </summary>
    public PaymentStatus Status { get; private set; }

    #region Foreign Keys

    /// <summary>
    /// شناسه مربوط به اینکه این درگاه پرداخت برای کدوم اگهیه؟
    /// </summary>
    public Guid AdvertisementId { get; private set; }

    /// <summary>
    /// شناسه مربوط به کارفرما پرداخت کننده 
    /// </summary>
    public Guid UserId { get; private set; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به اگهی که در درگاه پرداخت موجود است
    /// </summary>
    public virtual Advertisement Advertisement { get; private set; }

    /// <summary>
    /// جزئیات مربوط به کارفرما پرداخت کننده
    /// </summary>
    public virtual User User { get; private set; }

    #endregion

    protected override void Validate()
    {
        if (Amount < 0)
            throw new DomainException(DomainErrors.PayemntAmountOutOfRange);

        if (UserId == Guid.Empty)
            throw new DomainException(DomainErrors.PaymentUserIdIsRequired);

        if (AdvertisementId == Guid.Empty)
            throw new DomainException(DomainErrors.PaymentAdvertisementIdIsRequired);
    }

    public void UpdatePaymentStatus(PaymentStatus status, Guid? modifiedById)
    {
        Status = status;

        Update(modifiedById);
    }
}
