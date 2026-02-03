using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Domain.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Notifications.API.Controllers;

[Authorize]
[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class NotificationsController(ICreateNotificationUseCase createNotificationUseCase) : ControllerBase
{
    public async Task<ActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
    {
        var result = await createNotificationUseCase.CallAsync(dto);
        return result.Match(
            failure => FailureMapperExtension.MapFailure(this, failure),
            Ok
        );
    }
}