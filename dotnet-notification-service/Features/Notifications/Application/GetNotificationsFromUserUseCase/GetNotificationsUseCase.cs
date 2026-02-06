using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;

public class GetNotificationsUseCase : IGetNotificationsUseCase
{
    public Task<Either<Failure, GetNotificationsResult>> CallAsync(GetNotificationsCommand @params)
    {
        throw new NotImplementedException();
    }
}