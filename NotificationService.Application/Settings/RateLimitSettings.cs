namespace NotificationService.Application.Settings;

public class RateLimitSettings
{
    public int Limit { get; set; }
    public TimeSpan TimePeriod { get; set; }
}

public class RateLimitConfig
{
    public Dictionary<string, RateLimitSettings> RateLimits { get; set; }
}