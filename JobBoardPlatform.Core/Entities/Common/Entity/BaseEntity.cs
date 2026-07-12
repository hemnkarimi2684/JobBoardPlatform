using JobBoardPlatform.Core.Entities.UserEntity.Entity;

namespace JobBoardPlatform.Core.Entities.Common.Entity;

public abstract class BaseEntity : IEntity
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    /// <summary>
    /// شناسه موجودیت 
    /// </summary>
    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? ModifiedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool IsDeleted { get; private set; }

    #region Foreign Keys

    public Guid? CreatedById { get; protected set; }

    public Guid? ModifiedById { get; protected set; }

    public Guid? DeletedById { get; protected set; }

    #endregion

    #region Navigation Properties

    public User? Creator { get; private set; }

    public User? Modifier { get; private set; }

    public User? Deleter { get; private set; }

    #endregion

    public void SoftDelete(Guid deletedById)
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        ModifiedAt = DateTime.UtcNow;
        DeletedById = deletedById;
    }

    public void Update(Guid modifiedById)
    {
        ModifiedById = modifiedById;
        ModifiedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// متد مربوط به اعتبار سنجی کردن پراپرتی های موجودیت 
    /// </summary>
    protected abstract void Validate();
}

public interface IEntity
{
    /// <summary>
    /// زمان ساخت موجودیت در سیستم 
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// زمان تغییر موجودیت در سیستم 
    /// </summary>
    public DateTime? ModifiedAt { get; }

    /// <summary>
    /// زمان حذف نرم موجودیت از سیستم 
    /// </summary>
    public DateTime? DeletedAt { get; }

    /// <summary>
    /// ایا موجودیت حذف نرم شده یا نه
    /// </summary>
    public bool IsDeleted { get; }

    #region Foreign Keys

    /// <summary>
    /// موجودیت ساخته شده توسط 
    /// </summary>
    public Guid? CreatedById { get; }

    /// <summary>
    /// موجودیت اپدیت شده توسط 
    /// </summary>
    public Guid? ModifiedById { get; }

    /// <summary>
    /// موجودیت حذف شده توسط 
    /// </summary>
    public Guid? DeletedById { get; }

    #endregion

    #region Navigation Properties

    /// <summary>
    /// جزئیات مربوط به سازنده موجودیت 
    /// </summary>
    public User? Creator { get; }

    /// <summary>
    /// جزئیات مربوط به اپدیت کننده موجودیت 
    /// </summary>
    public User? Modifier { get; }

    /// <summary>
    /// جزئیات مربوط به حذف کننده موجودیت 
    /// </summary>
    public User? Deleter { get; }

    #endregion

    /// <summary>
    /// متد تغییر پراپرتی های مربوط به حذف نرم موجودیت
    /// </summary>
    public void SoftDelete(Guid deletedById);

    /// <summary>
    /// متد تغییر پراپرتی مربوط به اپدیت موجودیت
    /// </summary>
    public void Update(Guid modifiedById);
}