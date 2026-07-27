namespace JobBoardPlatform.Application.Interfaces.RedisInterface;

public interface IRedisService
{
    /// <summary>
    /// دریافت دیتا کچ شده 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// ست کردن دیتا مورد نظر 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiry"></param>
    /// <returns></returns>
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// حذف دیتا کش شده 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveAsync(string key);

    /// <summary>
    /// چک کردن دیتا کش شده با کلید مورد نظر 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(string key);
}

