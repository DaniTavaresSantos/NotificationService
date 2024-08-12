using Microsoft.AspNetCore.Mvc;
using NotificationService.ApplicationCore.Mappers;
using NotificationService.ApplicationCore.UseCases.Abstractions;
using NotificationService.Contracts;

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
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request, CancellationToken cancellationToken)
    {
        var response = await _notificationProcessor.Send(request.ToNotification(), cancellationToken);

        return response.IsSuccess ? Ok(response) : StatusCode(429, response);
    }
}