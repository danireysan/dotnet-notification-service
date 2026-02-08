using System.Text.Json.Serialization;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;

namespace dotnet_notification_service.Features.Notifications.API.DTOs;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(EmailNotificationDto), "email")]
[JsonDerivedType(typeof(SmsNotificationDto), "sms")]
[JsonDerivedType(typeof(PushNotificationDto), "push")]
public abstract record NotificationDto(
    string Title,
    string Content,
    string Recipient
)
{
    public abstract NotificationChannel Channel { get; }
};

public record EmailNotificationDto(string Title, string Content, string Email)
    : NotificationDto(Title, Content, Email)
{
    public override NotificationChannel Channel => NotificationChannel.Email;
}

public record SmsNotificationDto(
    string Title,
    string Content,
    string Phone
) : NotificationDto(Title, Content, Phone)
{
    public override NotificationChannel Channel => NotificationChannel.Sms;
}

public record PushNotificationDto(
    string Title,
    string Content,
    string DeviceId
) : NotificationDto(Title, Content, DeviceId)
{
    public override NotificationChannel Channel => NotificationChannel.Push;
}