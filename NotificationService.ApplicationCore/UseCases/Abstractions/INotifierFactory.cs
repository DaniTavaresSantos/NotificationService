using NotificationService.Domain;

namespace NotificationService.ApplicationCore.UseCases.Abstractions;

public interface INotifierFactory
{
    INotifierStrategy GetNotifierStrategy(Notification notification);
}