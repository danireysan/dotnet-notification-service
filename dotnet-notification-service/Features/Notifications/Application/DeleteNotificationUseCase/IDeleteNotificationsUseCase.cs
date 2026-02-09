using dotnet_notification_service.Core.Application;

namespace dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;

public interface IDeleteNotificationsUseCase : IUseCase<DeleteNotificationsResult, DeleteNotificationCommand>
{
    
}