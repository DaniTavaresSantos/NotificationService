using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Infra.Gateways.Strategies;

public class UnlimitedNotificationGateway : INotifierStrategy
{
    public LimitType LimitType => LimitType.Unlimited;

    public async Task<Result> Notify(Notification notification)
    {
        Console.WriteLine($"Sending message to user {notification.Recipient.EmailAdress}");
        
        //The code below is a simulation of a communication of this service with a Message broker or External Api, that is going to send the message effectively
        await Task.Delay(100);
        
        Console.WriteLine($"Message sent to user {notification.Recipient.EmailAdress}");

        return Result.Success();
    }
}