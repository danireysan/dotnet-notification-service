using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Domain.Repository;

public interface INotificationRepository
{
    Task<Either<Failure, Unit>> SaveNotification(NotificationEntity entity);
    Task<Either<Failure, Unit>> DeleteNotification(String id);
    Task<Either<Failure, Unit>> UpdateNotification(NotificationEntity entity);
    Task<Either<Failure, List<NotificationEntity>>> GetUserNotifications(string userid);
    
    Task<Option<bool>> VerifyNotificationIsFromUser(string notificationId, string userid);
}