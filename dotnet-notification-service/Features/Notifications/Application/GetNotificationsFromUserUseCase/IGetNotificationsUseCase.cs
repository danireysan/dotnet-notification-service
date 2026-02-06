using dotnet_notification_service.Core.Application;

namespace dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;

public interface IGetNotificationsUseCase : IUseCase<GetNotificationsResult, GetNotificationsCommand>
{
    
}