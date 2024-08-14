using NotificationService.Domain;
using Type = NotificationService.Domain.Type;

namespace NotificationService.Commons.Request;

public record NotificationRequest (Type? Type, Recipient Recipient, Message Message);