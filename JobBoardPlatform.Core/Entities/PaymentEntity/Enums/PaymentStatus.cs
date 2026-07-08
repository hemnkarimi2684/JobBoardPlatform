namespace JobBoardPlatform.Core.Entities.PaymentEntity.Enums;

public enum PaymentStatus
{
    /// <summary>
    /// در حال انتظار 
    /// </summary>
    Pending = 1,

    /// <summary>
    /// موفق شده
    /// </summary>
    Success,

    /// <summary>
    /// شکست خورده 
    /// </summary>
    Failed,

    /// <summary>
    /// کنسل شده 
    /// </summary>
    Cancelled
}
