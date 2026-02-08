using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Notifications.API.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotificationsController(
    ICreateNotificationUseCase createNotificationUseCase,
    IUpdateNotificationUseCase updateNotificationUseCase,
    ILogger<NotificationsController> logger
    ) : ControllerBase
{
    
    [HttpPost]
    public async Task<ActionResult> CreateNotification([FromBody] NotificationDto dto)
    {

        // Extracting the User ID (Subject)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value; 
        
        var email = User.FindFirstValue(ClaimTypes.Email);
        
        logger.LogDebug($"This is the mail{email} and this is the userId: {userId}");
        
        
        if (userId == null)
            return Unauthorized(
                "User if is not authenticated userId:"+ userId + email
                );

        var command = new CreateNotificationCommand(dto, userId);

        var result = await createNotificationUseCase.CallAsync(command);
        return result.Match(
            failure => FailureMapperExtension.MapFailure(this, failure),
            Ok
        );
    }

    [HttpPut]
    public async Task<ActionResult> UpdateNotification([FromBody] NotificationDto dto)
    {
        throw  new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult> GetNotifications()
    {
        throw   new NotImplementedException();
    }

    [HttpDelete("id")]
    public async Task<ActionResult> DeleteNotification(int id)
    {
        throw  new NotImplementedException();
    }
    
}