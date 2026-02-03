using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Domain.Repository;

public interface INotificationRepository
{
    Task<Either<Failure, Unit>> saveNotification(NotificationEntity command);
}