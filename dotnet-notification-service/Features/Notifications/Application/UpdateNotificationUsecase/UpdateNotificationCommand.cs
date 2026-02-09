using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;

namespace dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;

public record UpdateNotificationCommand(
    NotificationDto Data,
    string UserId
);