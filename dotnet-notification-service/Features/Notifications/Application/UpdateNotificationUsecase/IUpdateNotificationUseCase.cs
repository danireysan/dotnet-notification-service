using dotnet_notification_service.Core.Application;

namespace dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;

public interface IUpdateNotificationUseCase : IUseCase<UpdateNotificationResult, UpdateNotificationCommand>
{
    
}