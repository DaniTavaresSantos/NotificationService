using NotificationService.Commons;
using NotificationService.Contracts;
using NotificationService.Domain;

namespace NotificationService.ApplicationCore.UseCases.Abstractions;

public interface INotificationProcessor
{ 
    Task<Result> Send(Notification notification, CancellationToken cancellationToken);
}