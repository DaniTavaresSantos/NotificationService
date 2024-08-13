using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NotificationService.ApplicationCore.Settings;
using NotificationService.ApplicationCore.UseCases.Abstractions;
using NotificationService.Commons;
using NotificationService.Domain;
using NotificationService.Infra.Cache.Abstractions;

namespace NotificationService.ApplicationCore.UseCases;

public class RateLimitProcessor(ICacheService cache, RateLimitConfig rateLimitConfig) : IRateLimitProcessor
{
    private readonly Dictionary<string, RateLimitSettings> _rateLimits = rateLimitConfig.RateLimits;

    public bool IsNotificationAllowed(Notification notification)
    {
        if (!_rateLimits.ContainsKey(notification.Type.ToString())) return true;

        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        var cacheKey = $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}";
        
        var alreadyMadeNotifications = cache.GetData<int>(cacheKey);
        
        var notificationCount = 0;
        if(alreadyMadeNotifications != 0) 
            notificationCount = alreadyMadeNotifications;

        return notificationCount < rateLimitInfo.Limit;
    }

    public Result UpdateNotificationLimit(Notification notification)
    {
        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        var cacheKey = $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}";
        
        var alreadyMadeNotifications = cache.GetData<int>(cacheKey);
        
        var notificationCount = 0;
        if(alreadyMadeNotifications != 0) 
            notificationCount = alreadyMadeNotifications;

        notificationCount++;

        cache.SetData(cacheKey, notificationCount, rateLimitInfo.TimePeriod);
        
        return Result.Success();
    }
}