using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.Abstractions.UseCases;

public interface INotificationProcessor
{ 
    Task<Result> Send(Notification notification, CancellationToken cancellationToken);
}