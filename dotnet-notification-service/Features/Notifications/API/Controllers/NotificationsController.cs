using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;
using dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using Funcky.Monads;
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
    IDeleteNotificationsUseCase deleteNotificationUseCase,
    IGetNotificationsUseCase getNotificationsUseCase,
    ILogger<NotificationsController> logger
) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateNotification([FromBody] NotificationDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized("User is not authenticated");

        var command = new CreateNotificationCommand(dto, userId);

        var result = await createNotificationUseCase.CallAsync(command);

        return result.Match(
            failure => FailureMapperExtension.MapFailure(this, failure),
            Ok
        );
    }

    [HttpPut]
    public async Task<ActionResult> UpdateNotification([FromBody] NotificationDto updatable)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return Unauthorized("User is not authenticated");
        var command = new UpdateNotificationCommand(updatable, userId);

        var result = await updateNotificationUseCase.CallAsync(command);

        return result.Match(
            failure => FailureMapperExtension.MapFailure(this, failure),
            Ok
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized("User is not authenticated");

        var command = new GetNotificationsCommand(userId);
        var result = await getNotificationsUseCase.CallAsync(command);

        return result.Match(
            failure => FailureMapperExtension.MapFailure(this, failure),
            right: getNotificationsResult => Ok(getNotificationsResult.notifications)
        );
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteNotification(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized("User is not authenticated");
        
        var command = new DeleteNotificationCommand(userId, id);
        var result = await deleteNotificationUseCase.CallAsync(command);

        return result.Match(
            failure => FailureMapperExtension.MapFailure(this, failure),
            right: _ => NoContent()
        );
    }

    private Either<Failure, String> GetUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var failure = new UnauthorizedFailure
        {
            Message = "You are not authenticated"
        };
        if (userId == null) return Either<Failure, String>.Left(failure);
        return userId;
    }
}