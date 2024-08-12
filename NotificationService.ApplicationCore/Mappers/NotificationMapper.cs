using NotificationService.Contracts;
using NotificationService.Domain;

namespace NotificationService.ApplicationCore.Mappers;

public static class NotificationMapper
{
    public static Notification ToNotification(this NotificationRequest request)
    {
        return new Notification
        {
            Message = request.Message,
            Recipient = request.Recipient,
            Type = request.Type,
            LimitType = request.LimitType
        };
    }
}