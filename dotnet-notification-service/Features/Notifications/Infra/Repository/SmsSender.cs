using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class SmsSender : INotificationSender
{
    public NotificationChannel Channel => NotificationChannel.Sms;
    public Task<Either<Failure, Unit>> Send(NotificationEntity dto)
    {
        throw new NotImplementedException();
    }
}