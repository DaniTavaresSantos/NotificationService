using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infra.Cache;
using NotificationService.Infra.Cache.Abstractions;

namespace NotificationService.Infra.Settings;

[ExcludeFromCodeCoverage]
public static class InfraSettings
{
    public static void AddInfraSettings(this IServiceCollection services)
    {
        services.AddScoped<ICacheRepository, CacheRepository>();
    }
}