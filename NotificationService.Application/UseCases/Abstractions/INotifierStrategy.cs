using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Application.UseCases.Abstractions;

public interface INotifierStrategy
{
    LimitType LimitType { get; }
    Task<Result> Notify(Notification notification);
}