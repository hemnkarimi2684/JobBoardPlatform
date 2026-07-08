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

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        IsDeleted = true;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Update() => ModifiedAt = DateTime.UtcNow;

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

    /// <summary>
    /// متد تغییر پراپرتی های مربوط به حذف نرم موجودیت
    /// </summary>
    public void SoftDelete();

    /// <summary>
    /// متد تغییر پراپرتی مربوط به اپدیت موجودیت
    /// </summary>
    public void Update();
}