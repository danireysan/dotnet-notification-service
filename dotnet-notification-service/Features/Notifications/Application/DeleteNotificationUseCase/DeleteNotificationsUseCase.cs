using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;

public class DeleteNotificationsUseCase : IDeleteNotificationsUseCase
{
    public Task<Either<Failure, DeleteNotificationsResult>> CallAsync(DeleteNotificationsCommand @params)
    {
        throw new NotImplementedException();
    }
}