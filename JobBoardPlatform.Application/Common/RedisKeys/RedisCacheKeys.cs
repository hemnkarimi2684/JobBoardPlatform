namespace JobBoardPlatform.Application.Common.RedisKeys;

/// <summary>
/// کلیدهای کش ردیس همه کلیدهای کش برنامه
/// </summary>
public static class RedisCacheKeys
{
    /// <summary>
    /// لیست دسته بندی های شغلی برای دراپ داون بدون انقضا، بعد از هر تغییر ادمین حذف میشود
    /// </summary>
    public const string JobCategoriesSelect = "lookups:jobcategories:select";

    /// <summary>
    /// لیست شهرها برای دراپ داون بدون انقضا، بعد از هر تغییر ادمین حذف میشود
    /// </summary>
    public const string CitiesSelect = "lookups:cities:select";

    /// <summary>
    /// لیست استان ها برای دراپ داون بدون انقضا، بعد از هر تغییر ادمین حذف میشود
    /// </summary>
    public const string ProvincesSelect = "lookups:provinces:select";

    /// <summary>
    /// لیست مهارت ها برای دراپ داون بدون انقضا، بعد از هر تغییر ادمین حذف میشود
    /// </summary>
    public const string SkillsSelect = "lookups:skills:select";

    /// <summary>
    /// آمار داشبورد ادمین کش کوتاه مدت
    /// </summary>
    public const string AdminDashboardCounts = "admin:dashboard:counts";
}
