using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NotificationService.Infra.Cache.Abstractions;

namespace NotificationService.Infra.Cache;

public class CacheService: ICacheService
{
    private IDistributedCache _cacheService;
    public CacheService(IDistributedCache cacheService)
    {
        _cacheService = cacheService;
    }

    public T GetData<T>(string key)
    {
        var value = _cacheService.GetString(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);
        
        return default;
    }

    /// <summary>
    /// Set cache Value and its expiration Time Relative to now
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expirationTime">Time when cache expired</param>
    public void SetData<T>(string key, T value, TimeSpan expirationTime)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime
        };
        
        _cacheService.SetString(key, JsonSerializer.Serialize(value), options);

    }
}