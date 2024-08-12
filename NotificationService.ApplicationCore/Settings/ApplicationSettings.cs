using Microsoft.Extensions.DependencyInjection;
using NotificationService.ApplicationCore.UseCases;
using NotificationService.ApplicationCore.UseCases.Abstractions;

namespace NotificationService.ApplicationCore.Settings;

public static class ApplicationSettings
{
    public static void AddAppCoreSettings(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<IRateLimitProcessor, RateLimitProcessor>();
        services.AddScoped<INotificationProcessor, NotificationProcessor>();

        services.AddTransient<INotifierFactory, NotifierFactory>();

        services.AddTransient<INotifierStrategy, RateLimitedNotificationProcessor>();
        services.AddTransient<INotifierStrategy, UnlimitedNotificationProcessor>();
    }
}