using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Domain;

namespace NotificationService.Application.UseCases;

public class NotifierFactory : INotifierFactory
{
    private readonly IEnumerable<INotifierStrategy> _strategies;

    public NotifierFactory(IEnumerable<INotifierStrategy> strategies)
    {
        _strategies = strategies;
    }

    public INotifierStrategy GetNotifierStrategy(Notification notification)
    {
        var currentStrategy = _strategies.FirstOrDefault(x => x.LimitType == notification.LimitType);

        if (currentStrategy is null)
            throw new InvalidOperationException($"No supported Strategy for {notification.LimitType.ToString()}");

        return currentStrategy;
    }
}