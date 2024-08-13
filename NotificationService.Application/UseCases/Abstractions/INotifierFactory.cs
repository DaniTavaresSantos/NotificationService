using NotificationService.Domain;

namespace NotificationService.Application.UseCases.Abstractions;

public interface INotifierFactory
{
    INotifierStrategy GetNotifierStrategy(Notification notification);
}