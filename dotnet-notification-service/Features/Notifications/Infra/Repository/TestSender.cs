using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class TestSender : INotificationSender
{
    public NotificationChannel Channel => NotificationChannel.Email;
    public async Task<Either<Failure, SendResult>> Send(NotificationDto dto)
    {
        return new  SendResult(DateTime.UtcNow, null);
    }
}