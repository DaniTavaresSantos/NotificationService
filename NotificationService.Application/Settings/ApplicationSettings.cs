using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.UseCases;
using NotificationService.Application.UseCases.Abstractions;
using NotificationService.Application.UseCases.Strategies;

namespace NotificationService.Application.Settings;

[ExcludeFromCodeCoverage]
public static class ApplicationSettings
{
    public static void AddAppCoreSettings(this IServiceCollection services)
    {
        services.AddScoped<IRateLimitProcessor, RateLimitProcessor>();
        services.AddScoped<INotificationProcessor, NotificationProcessor>();

        services.AddTransient<INotifierFactory, NotifierFactory>();

        services.AddTransient<INotifierStrategy, RateLimitedNotificationProcessor>();
        services.AddTransient<INotifierStrategy, UnlimitedNotificationProcessor>();
    }
}