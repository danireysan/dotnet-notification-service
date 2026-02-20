namespace dotnet_notification_service.Features.Notifications.Domain.Entities;

public record SendResult(DateTime SentAt, string? Metadata);