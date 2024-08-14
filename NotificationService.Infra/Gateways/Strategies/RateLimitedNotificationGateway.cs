using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Commons;
using NotificationService.Domain;

namespace NotificationService.Infra.Gateways.Strategies;

public class RateLimitedNotificationGateway : INotifierStrategy
{
    private readonly IRateLimitProcessor _rateLimitProcessor;
    private readonly ILogger<RateLimitedNotificationGateway> _logger;

    public RateLimitedNotificationGateway(IRateLimitProcessor rateLimitProcessor, ILogger<RateLimitedNotificationGateway> logger)
    {
        _rateLimitProcessor = rateLimitProcessor;
        _logger = logger;
    }

    public LimitType LimitType => LimitType.RateLimited;

    public async Task<Result> Notify(Notification notification)
    {
        var cacheKey = $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}";
        
        if (_rateLimitProcessor.IsNotificationAllowed(notification, cacheKey))
        {
            Console.WriteLine($"Sending message of type {notification.Type.ToString()} to user: {notification.Recipient.EmailAdress}");
            
            //The code below is a simulation of a communication of this service with a Message broker or External Api, that is going to send the message effectively
            await Task.Delay(10);
            
            Console.WriteLine($"Message of type {notification.Type.ToString()} sent to user: {notification.Recipient.EmailAdress}");
            _logger.LogInformation("Notification of type {type} sent to {email}", notification.Type.ToString(), notification.Recipient.EmailAdress);
            
            _rateLimitProcessor.UpdateNotificationLimit(notification, cacheKey);
            return Result.Success();
        }
        
        _logger.LogWarning("Rate Limit of type {type} exceeded the limit for the user: {email}", 
            notification.Type.ToString(), notification.Recipient.EmailAdress);
        
        return Result.Failure(new Error("RateLimit.Exceeded", $"Rate Limit of type {notification.Type.ToString()} exceeded the limit for the user: {notification.Recipient.EmailAdress}"));
    }
}