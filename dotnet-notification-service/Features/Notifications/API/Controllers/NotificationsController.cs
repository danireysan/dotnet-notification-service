using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Notifications.API.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotificationsController(ICreateNotificationUseCase createNotificationUseCase) : ControllerBase
{
    
    [HttpPost]
    public async Task<ActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
    {

        // Extracting the User ID (Subject)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value; 
        
        var email = User.FindFirstValue(ClaimTypes.Email);
        
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
}