using Microsoft.Extensions.Caching.Memory;
using NotificationService.ApplicationCore.Settings;
using NotificationService.ApplicationCore.UseCases.Abstractions;
using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.ApplicationCore.UseCases;

public class RateLimitProcessor(IMemoryCache memoryCache, RateLimitConfig rateLimitConfig) : IRateLimitProcessor
{
    private readonly Dictionary<string, RateLimitSettings> _rateLimits = rateLimitConfig.RateLimits;

    public bool IsNotificationAllowed(Notification notification)
    {
        
        if (!_rateLimits.ContainsKey(notification.Type.ToString())) return true;

        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        var cacheKey = $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}";

        var notificationCount = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = rateLimitInfo.TimePeriod;
                return 0;
            }
        );

        return notificationCount < rateLimitInfo.Limit;
    }

    public Result UpdateNotificationLimit(Notification notification)
    {
        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        var cacheKey = $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}";

        var notificationCount = memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = rateLimitInfo.TimePeriod;
                return 0;
            }
        );
        
        notificationCount++;

        memoryCache.Set(cacheKey, notificationCount, rateLimitInfo.TimePeriod);
        return Result.Success();
    }
}