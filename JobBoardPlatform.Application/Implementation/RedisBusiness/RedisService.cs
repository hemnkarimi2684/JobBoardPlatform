using JobBoardPlatform.Application.Interfaces.RedisInterface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JobBoardPlatform.Application.Implementation.RedisBusiness;

public class RedisService : IRedisService
{
    private static readonly TimeSpan RedisCooldown = TimeSpan.FromSeconds(30);

    private static DateTime _nextRedisAttemptUtc;

    private readonly IDistributedCache _cache;

    private readonly ILogger<RedisService> _logger;

    public RedisService(
        IDistributedCache cache, 
        ILogger<RedisService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        if (IsRedisInCooldown())
            return false;

        try
        {
            return await _cache.GetStringAsync(key) is not null;
        }
        catch (Exception ex)
        {
            EnterCooldown();
            _logger.LogWarning(ex, "Redis is unavailable, falling back to database. Operation: {Operation}, Key: {Key}", nameof(ExistsAsync), key);

            return false;
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (IsRedisInCooldown())
            return default;

        try
        {
            var json = await _cache.GetStringAsync(key);

            if (json is null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            EnterCooldown();
            _logger.LogWarning(ex, "Redis is unavailable, falling back to database. Operation: {Operation}, Key: {Key}", nameof(GetAsync), key);

            return default;
        }
    }

    public async Task RemoveAsync(string key)
    {
        if (IsRedisInCooldown())
            return;

        try
        {
            await _cache.RemoveAsync(key);
        }
        catch (Exception ex)
        {
            EnterCooldown();
            _logger.LogWarning(ex, "Redis is unavailable, skipping cache removal. Operation: {Operation}, Key: {Key}", nameof(RemoveAsync), key);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        if (IsRedisInCooldown())
            return;

        try
        {
            var json = JsonSerializer.Serialize(value);

            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            });
        }
        catch (Exception ex)
        {
            EnterCooldown();
            _logger.LogWarning(ex, "Redis is unavailable, skipping cache write. Operation: {Operation}, Key: {Key}", nameof(SetAsync), key);
        }
    }

    private static bool IsRedisInCooldown() => DateTime.UtcNow < _nextRedisAttemptUtc;

    private static void EnterCooldown() => _nextRedisAttemptUtc = DateTime.UtcNow.Add(RedisCooldown);
}
