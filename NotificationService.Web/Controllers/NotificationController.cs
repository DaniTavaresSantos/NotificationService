using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Domain;
using NotificationService.Application.Mappers;
using NotificationService.Commons.Request;

namespace NotificationService.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController(INotificationProcessor notificationProcessor, IValidator<NotificationRequest> validator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SendNotification(
        [FromBody] [Required] NotificationRequest request,
        [FromHeader] [Required] LimitType limitType,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if(!validationResult.IsValid)
            return BadRequest(validationResult);
        
        var notification = request.ToNotification(limitType);
        
        var response = await notificationProcessor.Send(notification, cancellationToken);

        return response.IsSuccess ? Ok(response) : StatusCode(429, response);
    }
}