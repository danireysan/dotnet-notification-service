using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;

public class CreateNotificationUseCase(INotificationRepository repository, INotificationSender sender)
    : ICreateNotificationUseCase
{
    public async Task<Either<Failure, CreateNotificationResult>> CallAsync(CreateNotificationCommand @params)
    {
        
        var data = @params.Data;
        var entity = new NotificationEntity(data.Title, data.Content, data.Recipient, @params.UserId, data.Channel);
        var result = await repository.saveNotification(entity);


        return from _ in result
            select new CreateNotificationResult();
    }
}