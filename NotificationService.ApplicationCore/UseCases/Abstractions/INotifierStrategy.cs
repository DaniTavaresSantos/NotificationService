using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.ApplicationCore.UseCases.Abstractions;

public interface INotifierStrategy
{
    LimitType LimitType { get; }
    Task<Result> Notify(Notification notification);
}