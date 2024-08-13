using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.Abstractions.UseCases;

public interface IRateLimitProcessor
{
    bool IsNotificationAllowed(Notification notification, string cacheKey);

    Result UpdateNotificationLimit(Notification notification, string cacheKey);
}