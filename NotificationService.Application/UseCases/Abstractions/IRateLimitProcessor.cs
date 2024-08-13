using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.UseCases.Abstractions;

public interface IRateLimitProcessor
{
    bool IsNotificationAllowed(Notification notification, string cacheKey);

    Result UpdateNotificationLimit(Notification notification, string cacheKey);
}