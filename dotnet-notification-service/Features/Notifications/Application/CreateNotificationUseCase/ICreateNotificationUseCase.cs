using dotnet_notification_service.Core.Application;
using dotnet_notification_service.Features.Notifications.API.DTOs;

namespace dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase

{
    public interface ICreateNotificationUseCase : IUseCase<CreateNotificationResult, CreateNotificationCommand>

    {

    }
}

