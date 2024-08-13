using Microsoft.Extensions.Logging;
using Moq;
using NotificationService.Application.UseCases.Abstractions;
using NotificationService.Application.UseCases.Strategies;
using NotificationService.Domain;
using Xunit;
using Type = NotificationService.Domain.Type;

namespace NotificationService.Test.UseCases.Strategies;

public class RateLimitedNotificationProcessorTests
{
        private readonly Mock<IRateLimitProcessor> _mockRateLimitProcessor;
        private readonly Mock<ILogger<RateLimitedNotificationProcessor>> _mockLogger;
        private readonly RateLimitedNotificationProcessor _rateLimitedNotificationProcessor;

        public RateLimitedNotificationProcessorTests()
        {
            _mockRateLimitProcessor = new Mock<IRateLimitProcessor>();
            _mockLogger = new Mock<ILogger<RateLimitedNotificationProcessor>>();
            _rateLimitedNotificationProcessor = new RateLimitedNotificationProcessor(
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

            _mockRateLimitProcessor.Setup(r => r.IsNotificationAllowed(notification))
                                   .Returns(true);

            // Act
            var result = await _rateLimitedNotificationProcessor.Notify(notification);

            // Assert
            Assert.True(result.IsSuccess);
            _mockRateLimitProcessor.Verify(r => r.UpdateNotificationLimit(notification), Times.Once);
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

            _mockRateLimitProcessor.Setup(r => r.IsNotificationAllowed(notification))
                                   .Returns(false);

            // Act
            var result = await _rateLimitedNotificationProcessor.Notify(notification);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("RateLimit.Exceeded", result.Error.Code);
            _mockRateLimitProcessor.Verify(r => r.UpdateNotificationLimit(notification), Times.Never);
        }
}