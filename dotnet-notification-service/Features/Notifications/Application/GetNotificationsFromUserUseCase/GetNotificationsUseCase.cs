using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;

public class GetNotificationsUseCase(INotificationRepository repository) : IGetNotificationsUseCase
{
    public async Task<Either<Failure, GetNotificationsResult>> CallAsync(GetNotificationsCommand @params)
    {
        var result = await repository.GetUserNotifications(@params.userId);

        return from users in result
            select new GetNotificationsResult(MapNotificationDtos(users));
    }

    private static List<ResultNotificationDto> MapNotificationDtos(List<NotificationEntity> userNotificationEntities)
    {
        var list = userNotificationEntities.Select(nt =>
            new ResultNotificationDto(nt.NotificationId.ToString(), nt.Title, nt.Content, nt.Recipient, nt.Channel.ToString(), nt.SentAt.ToString() )).ToList();
        return list;
    }
}