using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class NotificationRepository : INotificationRepository
{
    public Task<Either<Failure, Unit>> SaveNotification(NotificationEntity entity)
    {
        throw new NotImplementedException();
    }
}