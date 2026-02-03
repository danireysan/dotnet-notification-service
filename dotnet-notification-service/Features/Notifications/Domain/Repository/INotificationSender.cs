using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Domain.Repository;

public interface INotificationSender
{
    Task<Either<Failure, Unit>> Send(NotificationEntity dto);
}