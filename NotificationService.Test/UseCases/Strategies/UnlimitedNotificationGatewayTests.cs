using NotificationService.Infra.Gateways.Strategies;
using NotificationService.Domain;
using Xunit;
using Type = NotificationService.Domain.Type;

namespace NotificationService.Test.UseCases.Strategies;

public class UnlimitedNotificationGatewayTests
{
    private readonly UnlimitedNotificationGateway _unlimitedNotificationGateway;

    public UnlimitedNotificationGatewayTests()
    {
        _unlimitedNotificationGateway = new UnlimitedNotificationGateway();
    }

    [Fact]
    public async Task Notify_ShouldCompleteSuccessfully_WhenCalled()
    {
        // Arrange
        var notification = new Notification
        {
            Type = null,
            Recipient = new Recipient { EmailAdress = "test@example.com" }
        };

        // Act
        var result = await _unlimitedNotificationGateway.Notify(notification);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Notify_ShouldOutputCorrectMessages_ToConsole()
    {
        // Arrange
        var notification = new Notification
        {
            Type = null,
            Recipient = new Recipient { EmailAdress = "test@example.com" }
        };

        using (var consoleOutput = new StringWriter())
        {
            Console.SetOut(consoleOutput);

            // Act
            await _unlimitedNotificationGateway.Notify(notification);

            // Assert
            var output = consoleOutput.ToString();
            Assert.Contains($"Sending message to user {notification.Recipient.EmailAdress}", output);
            Assert.Contains($"Message sent to user {notification.Recipient.EmailAdress}", output);
        }
    }
}