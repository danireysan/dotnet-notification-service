namespace dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;

public record NotificationEntity(
    string Title,
    string Content,
    string Recipient,
    string CreatedBy,
    NotificationChannel Channel
);