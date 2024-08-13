using NotificationService.Domain;

namespace NotificationService.Application.Abstractions.UseCases;

public interface INotifierFactory
{
    INotifierStrategy GetNotifierStrategy(Notification notification);
}