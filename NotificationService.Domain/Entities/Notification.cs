namespace NotificationService.Domain;

public class Notification
{
    public LimitType LimitType { get; set; }
    public Type? Type { get; set; }
    public Recipient Recipient { get; set; }
    public Message Message { get; set; }
}