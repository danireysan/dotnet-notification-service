using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Notifications.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotificationsController(ICreateNotificationUseCase createNotificationUseCase) : ControllerBase
{
        

    public async Task<ActionResult> CreateNotification([FromBody] CreateNotificationCommand command)
    {
        var result = await createNotificationUseCase.CallAsync(command);
        return result.Match<ActionResult>(
            left: failure =>  BadRequest(), 
            right: notificationResult => Created() 
        );
    }
}