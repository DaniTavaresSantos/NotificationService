using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.UseCases;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Application.Validators;

namespace NotificationService.Application.Settings;

[ExcludeFromCodeCoverage]
public static class ApplicationSettings
{
    public static void AddAppCoreSettings(this IServiceCollection services)
    {
        services.AddScoped<IRateLimitProcessor, RateLimitProcessor>();
        services.AddScoped<INotificationProcessor, NotificationProcessor>();

        services.AddTransient<INotifierFactory, NotifierFactory>();
        
        services.AddValidatorsFromAssemblyContaining<NotificationRequestValidator>();
    }
}