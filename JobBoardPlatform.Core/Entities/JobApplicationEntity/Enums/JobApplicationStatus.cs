namespace JobBoardPlatform.Core.Entities.JobApplicationEntity.Enums;

public enum JobApplicationStatus 
{
    /// <summary>
    /// در حال انتظار
    /// </summary>
    Pending = 1,

    /// <summary>
    /// در حال بررسی
    /// </summary>
    Reviewing,

    /// <summary>
    /// در مرحله مصاحبه
    /// </summary>
    Interview,

    /// <summary>
    /// قبول شده
    /// </summary>
    Accepted,

    /// <summary>
    /// رد شده
    /// </summary>
    Rejected,

    /// <summary>
    /// کنسل شده
    /// </summary>
    Cancelled
}
