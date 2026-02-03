using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class NotificationRepository(NotificationContext context) : INotificationRepository
{
    public async Task<Either<Failure, Unit>> SaveNotification(NotificationEntity entity)
    {
        try
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            
            return new Unit();
        }
        catch (Exception e)
        {
            // Extract the most specific error message available
            var detailedMessage = e.InnerException?.Message ?? e.Message;

            var failure = new ServerFailure
            {
                Message = $"Save notification failed because: {detailedMessage}",
            };
            return Either<Failure, Unit>.Left(failure);
        }
    }
}