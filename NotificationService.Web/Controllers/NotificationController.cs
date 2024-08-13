using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Abstractions.UseCases;
using NotificationService.Domain;
using NotificationService.Application.Mappers;
using NotificationService.Commons.Request;

namespace NotificationService.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationProcessor _notificationProcessor;

    public NotificationController(INotificationProcessor notificationProcessor)
    {
        _notificationProcessor = notificationProcessor;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification(
        [FromBody] [Required] NotificationRequest request,
        [FromHeader] [Required] LimitType limitType,
        CancellationToken cancellationToken)
    {
        var notification = request.ToNotification(limitType);
        
        var response = await _notificationProcessor.Send(notification, cancellationToken);

        return response.IsSuccess ? Ok(response) : StatusCode(429, response);
    }
}