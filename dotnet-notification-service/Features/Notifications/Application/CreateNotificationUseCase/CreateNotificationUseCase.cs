using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;
using NUlid;

namespace dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;

public class CreateNotificationUseCase(INotificationRepository repository, INotificationSender sender)
    : ICreateNotificationUseCase
{
    public async Task<Either<Failure, CreateNotificationResult>> CallAsync(CreateNotificationCommand @params)
    {
        var ulid = new Ulid();
        var data = @params.Data;
        var entity = new NotificationEntity(ulid, data.Title, data.Content, data.Recipient, @params.UserId, data.Channel);
        var result = await repository.SaveNotification(entity);


        return from _ in result
            select new CreateNotificationResult();
    }
}