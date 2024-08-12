using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infra.Cache;
using NotificationService.Infra.Cache.Abstractions;

namespace NotificationService.Infra.Settings;

public static class InfraSettings
{
    public static void AddInfraSettings(this IServiceCollection services)
    {
        services.AddScoped<ICacheService, CacheService>();
    }
}