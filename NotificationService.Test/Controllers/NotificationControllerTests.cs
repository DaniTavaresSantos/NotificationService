using AutoBogus;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Commons;
using NotificationService.Commons.Request;
using NotificationService.Domain;
using NotificationService.Web.Controllers;
using Xunit;

namespace NotificationService.Test.Controllers;

public class NotificationControllerTests
{
        private readonly Mock<INotificationProcessor> _mockNotificationProcessor;
        private readonly Mock<IValidator<NotificationRequest>> _mockValidatorRequest;
        private readonly NotificationController _controller;

        public NotificationControllerTests()
        {
            _mockNotificationProcessor = new Mock<INotificationProcessor>();
            _mockValidatorRequest = new Mock<IValidator<NotificationRequest>>();
            
            _controller = new NotificationController(_mockNotificationProcessor.Object, _mockValidatorRequest.Object);
        }

        [Fact]
        public async Task SendNotification_ShouldReturnOk_WhenProcessorReturnsSuccess()
        {
            // Arrange
            var request = CreateRequest();
            var limitType = LimitType.RateLimited; 
            var result = Result.Success();

            _mockNotificationProcessor.Setup(p => p.Send(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(result);
            _mockValidatorRequest.Setup(x => x.ValidateAsync(It.IsAny<NotificationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

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
            var request = CreateRequest();
            var limitType = LimitType.RateLimited;
            var result = Result.Failure(new Error("RateLimit.Exceeded", "Rate limit exceeded"));

            _mockNotificationProcessor.Setup(p => p.Send(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            
            _mockValidatorRequest.Setup(x => x.ValidateAsync(It.IsAny<NotificationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var actionResult = await _controller.SendNotification(request, limitType, CancellationToken.None);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(429, statusCodeResult.StatusCode);
        }
        
        [Fact]
        public async Task SendNotification_ShouldReturnBadRequest_WhenRequestIsNotValid()
        {
            // Arrange
            var request = CreateRequest();
            var limitType = LimitType.Unlimited; 
            var result = Result.Success();

            _mockNotificationProcessor.Setup(p => p.Send(It.IsAny<Notification>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(result);
            _mockValidatorRequest.Setup(x => x.ValidateAsync(It.IsAny<NotificationRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    Capacity = 1
                }));

            // Act
            var actionResult = await _controller.SendNotification(request, limitType, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(result, okResult.Value);
        }

        private NotificationRequest CreateRequest()
        {
            var emailAddress = "dani.teste@gmail.com";

            var recipient = new AutoFaker<Recipient>()
                .RuleFor(x => x.EmailAdress, _ => emailAddress);

            
            var request = new AutoFaker<NotificationRequest>()
                .RuleFor(x => x.Message, _ => AutoFaker.Generate<Message>())
                .RuleFor(x => x.Recipient, _ => recipient);
            

            return request;
        }
}