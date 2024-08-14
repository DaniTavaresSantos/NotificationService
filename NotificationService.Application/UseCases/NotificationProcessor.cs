using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.UseCases;

public class NotificationProcessor(INotifierFactory notifierFactory, ILogger<NotificationProcessor> logger) : INotificationProcessor
{
    public async Task<Result> Send(Notification notification, CancellationToken cancellationToken)
    {
        var notifier = notifierFactory.GetNotifierStrategy(notification);
        var result = await notifier.Notify(notification);

        return result;
    }
}