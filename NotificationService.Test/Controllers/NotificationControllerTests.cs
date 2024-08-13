using System.Runtime.CompilerServices;
using AutoBogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationService.ApplicationCore.UseCases.Abstractions;
using NotificationService.Commons;
using NotificationService.Contracts;
using NotificationService.Domain;
using NotificationService.Web.Controllers;
using Xunit;

namespace NotificationService.Test.Controllers;

public class NotificationControllerTests
{
        private readonly Mock<INotificationProcessor> _mockNotificationProcessor;
        private readonly NotificationController _controller;

        public NotificationControllerTests()
        {
            _mockNotificationProcessor = new Mock<INotificationProcessor>();
            _controller = new NotificationController(_mockNotificationProcessor.Object);
        }

        [Fact]
        public async Task SendNotification_ShouldReturnOk_WhenProcessorReturnsSuccess()
        {
            // Arrange
            var request = AutoFaker.Generate<NotificationRequest>(); 
            var limitType = LimitType.RateLimited; 
            var result = Result.Success();

            _mockNotificationProcessor.Setup(p => p.Send(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(result);

            // Act
            var actionResult = await _controller.SendNotification(request, limitType, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(result, okResult.Value);
        }

        [Fact]
        public async Task SendNotification_ShouldReturnRateLimitExceeded_WhenProcessorReturnsFailure()
        {
            // Arrange
            var request = AutoFaker.Generate<NotificationRequest>();
            var limitType = LimitType.RateLimited;
            var result = Result.Failure(new Error("RateLimit.Exceeded", "Rate limit exceeded"));

            _mockNotificationProcessor.Setup(p => p.Send(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _controller.SendNotification(request, limitType, CancellationToken.None);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(429, statusCodeResult.StatusCode);
        }
}