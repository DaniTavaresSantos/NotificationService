using NotificationService.Commons.Request;
using NotificationService.Domain;

namespace NotificationService.Application.Mappers;

public static class NotificationMapper
{
    public static Notification ToNotification(this NotificationRequest request, LimitType limitType)
    {
        return new Notification
        {
            Message = request.Message,
            Recipient = request.Recipient,
            Type = request.Type,
            LimitType = limitType
        };
    }
}