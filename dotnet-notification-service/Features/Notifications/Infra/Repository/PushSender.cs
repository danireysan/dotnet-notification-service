using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using FirebaseAdmin.Messaging;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class PushSender : INotificationSender
{
    public NotificationChannel Channel => NotificationChannel.Push;
    public async Task<Either<Failure, SendResult>> Send(NotificationDto dto)
    {
        try
        {
            var message = new Message()
            {
                Notification = new Notification
                {
                    Title = dto.Title,
                    Body = dto.Content,
                },
                Token = dto.Recipient
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message);

            var date = DateTime.UtcNow;
            return new SendResult(date);
        }
        catch (Exception e)
        {
            var detailedMessage = e.InnerException?.Message ?? e.Message;
            var failure = new ServerFailure
            {
                Message = $"Push send failed because: {detailedMessage}"
            };

            return Either<Failure, SendResult>.Left(failure);
        }
    }
}