using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.ApplicationCore.UseCases.Abstractions;

public interface IRateLimitProcessor
{
    bool IsNotificationAllowed(Notification notification);

    Result UpdateNotificationLimit(Notification notification);
}