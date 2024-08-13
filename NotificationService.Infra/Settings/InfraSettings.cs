using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Abstractions.Repositories;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Infra.Cache;
using NotificationService.Infra.Gateways.Strategies;

namespace NotificationService.Infra.Settings;

[ExcludeFromCodeCoverage]
public static class InfraSettings
{
    public static void AddInfraSettings(this IServiceCollection services)
    {
        services.AddScoped<IRateLimitRepository, RateLimitRepository>();
        
        services.AddTransient<INotifierStrategy, RateLimitedNotificationGateway>();
        services.AddTransient<INotifierStrategy, UnlimitedNotificationGateway>();
    }
}