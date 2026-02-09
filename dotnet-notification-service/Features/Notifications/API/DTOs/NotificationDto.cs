using System.Text.Json.Serialization;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;

namespace dotnet_notification_service.Features.Notifications.API.DTOs;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "Type")]
[JsonDerivedType(typeof(EmailNotificationDto), "email")]
[JsonDerivedType(typeof(SmsNotificationDto), "sms")]
[JsonDerivedType(typeof(PushNotificationDto), "push")]
public abstract record NotificationDto(
    string? Id,
    string Title,
    string Content,
    string Recipient
)
{
    public abstract NotificationChannel Channel { get; }
};

public record EmailNotificationDto(string? Id, string Title, string Content, string Recipient)
    : NotificationDto(Id, Title, Content, Recipient)
{
    public override NotificationChannel Channel => NotificationChannel.Email;
}

public record SmsNotificationDto(
    string? Id,
    string Title,
    string Content,
    string Recipient
) : NotificationDto(Id, Title, Content, Recipient)
{
    public override NotificationChannel Channel => NotificationChannel.Sms;
}

public record PushNotificationDto(
    string? Id,
    string Title,
    string Content,
    string Recipient
) : NotificationDto(Id, Title, Content, Recipient)
{
    public override NotificationChannel Channel => NotificationChannel.Push;
}

public record ResultNotificationDto(
    string Id,
    string Title,
    string Content,
    string Recipient
);

