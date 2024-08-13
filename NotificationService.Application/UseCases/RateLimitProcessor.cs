using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Application.Settings;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.UseCases;

public class RateLimitProcessor(IRateLimitRepository rateLimit, RateLimitConfig rateLimitConfig) : IRateLimitProcessor
{
    private readonly Dictionary<string, RateLimitSettings> _rateLimits = rateLimitConfig.RateLimits;

    public bool IsNotificationAllowed(Notification notification, string cacheKey)
    {
        if (!_rateLimits.ContainsKey(notification.Type.ToString())) return true;

        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        
        var alreadyMadeNotifications = rateLimit.GetData<int>(cacheKey);

        return alreadyMadeNotifications < rateLimitInfo.Limit;
    }

    public Result UpdateNotificationLimit(Notification notification, string cacheKey)
    {
        var rateLimitInfo = _rateLimits[notification.Type.ToString()];
        
        var alreadyMadeNotifications = rateLimit.GetData<int>(cacheKey);

        alreadyMadeNotifications++;

        rateLimit.SetData(cacheKey, alreadyMadeNotifications, rateLimitInfo.TimePeriod);
        
        return Result.Success();
    }
}