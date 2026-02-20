using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;
using NUlid;

namespace dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;

using UpdateEither = Either<Failure, UpdateNotificationResult>;

public class UpdateNotificationUseCase(INotificationRepository repository) : IUpdateNotificationUseCase
{
    public async Task<UpdateEither> CallAsync(UpdateNotificationCommand @params)
    {
        var notificationId = @params.Data.Id;
        var notFoundFailure = new NotFoundFailure
        {
            Message = "This notification doesn't exist. ID: " + notificationId
        };
        if (notificationId is null) return UpdateEither.Left(notFoundFailure);
        var notificationResult = await repository.GetNotificationById(notificationId);
        var oldNotification = notificationResult.RightOrNone().ToNullable();

        if (oldNotification is null) return UpdateEither.Left(notFoundFailure);

        var newNotification = new NotificationEntity(
            Ulid.Parse(@params.Data.Id),
            @params.Data.Title,
            @params.Data.Content,
            @params.Data.Recipient,
            @params.UserId,
            @params.Data.Channel,
            oldNotification.SentAt
        );
        
        var result = await repository.UpdateNotification(newNotification);
        return
            from isUserNotification in repository.IsNotificationFromUser(oldNotification, @params.UserId)
            from _ in result
            select new UpdateNotificationResult();
    }
}