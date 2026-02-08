using dotnet_notification_service.Features.Notifications.API.DTOs;

namespace dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;

public record CreateNotificationCommand(
    NotificationDto Data,
    string UserId
);