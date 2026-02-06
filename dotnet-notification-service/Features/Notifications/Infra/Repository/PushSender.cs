using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class PushSender : INotificationSender
{
    public NotificationChannel Channel => NotificationChannel.Push;
    public Task<Either<Failure, Unit>> Send(NotificationEntity dto)
    {
        throw new NotImplementedException();
    }
}