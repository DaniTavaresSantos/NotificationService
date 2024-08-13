using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.UseCases.Abstractions;

public interface INotificationProcessor
{ 
    Task<Result> Send(Notification notification, CancellationToken cancellationToken);
}