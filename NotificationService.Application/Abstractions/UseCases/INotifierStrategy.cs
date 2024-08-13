using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.Abstractions.UseCases;

public interface INotifierStrategy
{
    LimitType LimitType { get; }
    Task<Result> Notify(Notification notification);
}