using Moq;
using Microsoft.Extensions.Logging;
using NotificationService.ApplicationCore.UseCases;
using NotificationService.ApplicationCore.UseCases.Abstractions;
using NotificationService.Commons;
using NotificationService.Domain;
using Xunit;
using AutoBogus;

namespace NotificationService.Test.UseCases;

public class NotificationProcessorTests
{
        private readonly Mock<INotifierFactory> _mockNotifierFactory;
        private readonly Mock<ILogger<NotificationProcessor>> _mockLogger;
        private readonly NotificationProcessor _notificationProcessor;

        public NotificationProcessorTests()
        {
            _mockNotifierFactory = new Mock<INotifierFactory>();
            _mockLogger = new Mock<ILogger<NotificationProcessor>>();
            _notificationProcessor = new NotificationProcessor(
                _mockNotifierFactory.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Send_ShouldReturnSuccessResult_WhenNotifierReturnsSuccess()
        {
            // Arrange
            var notification = AutoFaker.Generate<Notification>();
            var mockNotifier = new Mock<INotifierStrategy>();
            mockNotifier.Setup(n => n.Notify(notification))
                        .ReturnsAsync(Result.Success());

            _mockNotifierFactory.Setup(f => f.GetNotifierStrategy(notification))
                                .Returns(mockNotifier.Object);

            // Act
            var result = await _notificationProcessor.Send(notification, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            mockNotifier.Verify(n => n.Notify(notification), Times.Once);
        }

        [Fact]
        public async Task Send_ShouldReturnFailureResult_WhenNotifierReturnsFailure()
        {
            // Arrange
            var expectedResult = Result.Failure(new Error("Error code", "Error message"));
            var notification = AutoFaker.Generate<Notification>();
            var mockNotifier = new Mock<INotifierStrategy>();
            mockNotifier.Setup(n => n.Notify(notification))
                        .ReturnsAsync(expectedResult);

            _mockNotifierFactory.Setup(f => f.GetNotifierStrategy(notification))
                                .Returns(mockNotifier.Object);

            // Act
            var result = await _notificationProcessor.Send(notification, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedResult.Error, result.Error);
            mockNotifier.Verify(n => n.Notify(notification), Times.Once);
        }

        [Fact]
        public async Task Send_ShouldCallNotifierFactoryOnce()
        {
            // Arrange
            var notification = AutoFaker.Generate<Notification>();
            var mockNotifier = new Mock<INotifierStrategy>();
            mockNotifier.Setup(n => n.Notify(notification))
                        .ReturnsAsync(Result.Success());

            _mockNotifierFactory.Setup(f => f.GetNotifierStrategy(notification))
                                .Returns(mockNotifier.Object);

            // Act
            await _notificationProcessor.Send(notification, CancellationToken.None);

            // Assert
            _mockNotifierFactory.Verify(f => f.GetNotifierStrategy(notification), Times.Once);
        }
}