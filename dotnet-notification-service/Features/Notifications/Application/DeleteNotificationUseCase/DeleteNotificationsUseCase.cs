using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;

using EitherDelete = Either<Failure, DeleteNotificationsResult>;

public class DeleteNotificationsUseCase(INotificationRepository repository) : IDeleteNotificationsUseCase
{
    public async Task<EitherDelete> CallAsync(DeleteNotificationCommand @params)
    {
        var getNotificationResult = (await repository.GetNotificationById(@params.notificationId));
        
        var result = await repository.DeleteNotification(@params.notificationId);
        return 
            from notification in getNotificationResult
            from isUserNotification in repository.IsNotificationFromUser(notification, @params.userId)
            from _ in result
            select new DeleteNotificationsResult();
    }
}