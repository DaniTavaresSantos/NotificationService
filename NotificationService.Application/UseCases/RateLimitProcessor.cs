using NotificationService.Application.Settings;
using NotificationService.Application.UseCases.Abstractions;
using NotificationService.Commons;
using NotificationService.Domain;
using NotificationService.Infra.Cache.Abstractions;

namespace NotificationService.Application.UseCases;

public class RateLimitProcessor(ICacheRepository cache, RateLimitConfig rateLimitConfig) : IRateLimitProcessor
{
    private readonly Dictionary<string, RateLimitSettings> _rateLimits = rateLimitConfig.RateLimits;

    public bool IsNotificationAllowed(Notification notification, string cacheKey)
    {
        if (!_rateLimits.ContainsKey(notification.Type.ToString())) return true;

        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        
        var alreadyMadeNotifications = cache.GetData<int>(cacheKey);

        return alreadyMadeNotifications < rateLimitInfo.Limit;
    }

    public Result UpdateNotificationLimit(Notification notification, string cacheKey)
    {
        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        
        var alreadyMadeNotifications = cache.GetData<int>(cacheKey);

        alreadyMadeNotifications++;

        cache.SetData(cacheKey, alreadyMadeNotifications, rateLimitInfo.TimePeriod);
        
        return Result.Success();
    }
}