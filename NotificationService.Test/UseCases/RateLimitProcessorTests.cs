using Moq;
using NotificationService.Application.Settings;
using NotificationService.Application.UseCases;
using NotificationService.Domain;
using NotificationService.Infra.Cache.Abstractions;
using Xunit;
using Type = NotificationService.Domain.Type;

namespace NotificationService.Test.UseCases;

public class RateLimitProcessorTests
{
        private readonly Mock<ICacheRepository> _mockCacheService;
        private readonly RateLimitProcessor _rateLimitProcessor;
        private readonly RateLimitConfig _rateLimitConfig;

        public RateLimitProcessorTests()
        {
            _mockCacheService = new Mock<ICacheRepository>();
            
            var rateLimits = new Dictionary<string, RateLimitSettings>
            {
                { "Status", new RateLimitSettings { Limit = 2, TimePeriod = TimeSpan.FromMinutes(1) } },
                { "News", new RateLimitSettings { Limit = 1, TimePeriod = TimeSpan.FromDays(1) } }
            };

            _rateLimitConfig = new RateLimitConfig { RateLimits = rateLimits };
            _rateLimitProcessor = new RateLimitProcessor(_mockCacheService.Object, _rateLimitConfig);
        }

        [Fact]
        public void IsNotificationAllowed_ShouldReturnTrue_WhenNoRateLimitForType()
        {
            // Arrange
            var notification = new Notification
            {
                Type = Type.Marketing,
                Recipient = new Recipient { EmailAdress = "dani@example.com" }
            };

            // Act
            var result = _rateLimitProcessor.IsNotificationAllowed(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNotificationAllowed_ShouldReturnTrue_WhenNotificationCountIsBelowLimit()
        {
            // Arrange
            var notification = new Notification
            {
                Type = Type.Status,
                Recipient = new Recipient { EmailAdress = "test@example.com" }
            };

            _mockCacheService.Setup(cache => cache.GetData<int>("test@example.com:Status"))
                             .Returns(1); // Existing count is below the limit

            // Act
            var result = _rateLimitProcessor.IsNotificationAllowed(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsNotificationAllowed_ShouldReturnFalse_WhenNotificationCountExceedsLimit()
        {
            // Arrange
            var notification = new Notification
            {
                Type = Type.Status,
                Recipient = new Recipient { EmailAdress = "test@example.com" }
            };

            _mockCacheService.Setup(cache => cache.GetData<int>("test@example.com:Status"))
                             .Returns(3); // Existing count equals the limit

            // Act
            var result = _rateLimitProcessor.IsNotificationAllowed(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateNotificationLimit_ShouldUpdateCache_WhenCalled()
        {
            // Arrange
            var notification = new Notification
            {
                Type = Type.News,
                Recipient = new Recipient { EmailAdress = "test@example.com" }
            };

            _mockCacheService.Setup(cache => cache.GetData<int>("test@example.com:News"))
                             .Returns(3); // Existing count is 3

            // Act
            var result = _rateLimitProcessor.UpdateNotificationLimit(notification, $"{notification.Recipient.EmailAdress}:{notification.Type.ToString()}");

            // Assert
            _mockCacheService.Verify(cache => cache.SetData("test@example.com:News", 4, It.IsAny<TimeSpan>()), Times.Once);
            Assert.True(result.IsSuccess);
        }
}