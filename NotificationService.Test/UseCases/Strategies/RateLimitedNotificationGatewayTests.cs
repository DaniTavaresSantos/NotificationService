using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Infra.Gateways.Strategies;
using NotificationService.Domain;
using Xunit;
using Type = NotificationService.Domain.Type;

namespace NotificationService.Test.UseCases.Strategies;

public class RateLimitedNotificationGatewayTests
{
        private readonly Mock<IRateLimitProcessor> _mockRateLimitProcessor;
        private readonly Mock<ILogger<RateLimitedNotificationGateway>> _mockLogger;
        private readonly RateLimitedNotificationGateway _rateLimitedNotificationGateway;

        public RateLimitedNotificationGatewayTests()
        {
            _mockRateLimitProcessor = new Mock<IRateLimitProcessor>();
            _mockLogger = new Mock<ILogger<RateLimitedNotificationGateway>>();
            _rateLimitedNotificationGateway = new RateLimitedNotificationGateway(
                _mockRateLimitProcessor.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Notify_ShouldSendNotification_WhenRateLimitAllows()
        {
            // Arrange
            var notification = new Notification
            {
                Type = Type.Status,
                Recipient = new Recipient { EmailAdress = "test@example.com" }
            };

            _mockRateLimitProcessor.Setup(r => r.IsNotificationAllowed(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}"))
                                   .Returns(true);

            // Act
            var result = await _rateLimitedNotificationGateway.Notify(notification);

            // Assert
            Assert.True(result.IsSuccess);
            _mockRateLimitProcessor.Verify(r => r.UpdateNotificationLimit(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}"), Times.Once);
        }

        [Fact]
        public async Task Notify_ShouldNotSendNotification_WhenRateLimitExceeds()
        {
            // Arrange
            var notification = new Notification
            {
                Type = Type.Status,
                Recipient = new Recipient { EmailAdress = "test@example.com" }
            };

            _mockRateLimitProcessor.Setup(r => r.IsNotificationAllowed(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}"))
                                   .Returns(false);

            // Act
            var result = await _rateLimitedNotificationGateway.Notify(notification);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("RateLimit.Exceeded", result.Error.Code);
            _mockRateLimitProcessor.Verify(r => r.UpdateNotificationLimit(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}"), Times.Never);
        }
}