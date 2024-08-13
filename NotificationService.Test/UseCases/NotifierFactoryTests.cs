using Moq;
using NotificationService.Application.UseCases;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Domain;
using Xunit;

namespace NotificationService.Test.UseCases;

public class NotifierFactoryTests
{

    public NotifierFactoryTests()
    {
        
    }

    [Fact]
    public void GetNotifierStrategy_ShouldReturnCorrectStrategy_WhenValidLimitTypeIsProvided()
    {
        // Arrange
        var notification = new Notification { LimitType = LimitType.Unlimited };

        var strategy1 = new Mock<INotifierStrategy>();
        strategy1.SetupGet(x => x.LimitType).Returns(notification.LimitType);
        var strategy2 = new Mock<INotifierStrategy>();
        strategy2.SetupGet(x => x.LimitType).Returns(LimitType.RateLimited);
        
        var mockStrategies = new List<INotifierStrategy>
        {
            strategy1.Object,
            strategy2.Object
        };

        var notifierFactory = new NotifierFactory(mockStrategies);

        // Act
        var strategy = notifierFactory.GetNotifierStrategy(notification);

        // Assert
        Assert.NotNull(strategy);
        Assert.Equal(LimitType.Unlimited, strategy.LimitType);
    }

    [Fact]
    public void GetNotifierStrategy_ShouldThrowInvalidOperationException_WhenNoMatchingStrategyIsFound()
    {
        // Arrange
        
        var notification = new Notification { LimitType = LimitType.RateLimited };

        var strategy1 = new Mock<INotifierStrategy>();
        strategy1.SetupGet(x => x.LimitType).Returns(LimitType.Unlimited);
        var strategy2 = new Mock<INotifierStrategy>();
        strategy2.SetupGet(x => x.LimitType).Returns(LimitType.Unlimited);
        
        var mockStrategies = new List<INotifierStrategy>
        {
            strategy1.Object,
            strategy2.Object
        };

        var notifierFactory = new NotifierFactory(mockStrategies);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => notifierFactory.GetNotifierStrategy(notification));
        Assert.Contains("No supported Strategy for", exception.Message);
    }

    [Fact]
    public void GetNotifierStrategy_ShouldReturnCorrectStrategy_WhenMultipleStrategiesArePresent()
    {
        // Arrange
        var notification = new Notification { LimitType = LimitType.RateLimited };

        var strategy1 = new Mock<INotifierStrategy>();
        strategy1.SetupGet(x => x.LimitType).Returns(notification.LimitType);
        var strategy2 = new Mock<INotifierStrategy>();
        strategy2.SetupGet(x => x.LimitType).Returns(LimitType.Unlimited);
        
        var mockStrategies = new List<INotifierStrategy>
        {
            strategy1.Object,
            strategy2.Object
        };

        var notifierFactory = new NotifierFactory(mockStrategies);

        // Act
        var strategy = notifierFactory.GetNotifierStrategy(notification);

        // Assert
        Assert.NotNull(strategy);
        Assert.Equal(LimitType.RateLimited, strategy.LimitType);
    }
}