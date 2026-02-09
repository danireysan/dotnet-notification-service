using dotnet_notification_service.Core.Application;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;

namespace dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;

public interface IGetNotificationsUseCase : IUseCase<GetNotificationsResult, GetNotificationsCommand>
{
    
}