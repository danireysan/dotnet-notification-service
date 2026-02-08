using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;
using Microsoft.EntityFrameworkCore;

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
            var detailedMessage = e.InnerException?.Message ?? e.Message;

            var failure = new ServerFailure
            {
                Message = $"Save notification failed because: {detailedMessage}",
            };
            return Either<Failure, Unit>.Left(failure);
        }
    }

    public async Task<Either<Failure, Unit>> DeleteNotification(string id)
    {
        try
        {
            context.Remove(id);
            await context.SaveChangesAsync();
            return new Unit();
        }
        catch (Exception e)
        {
            var detailedMessage = e.InnerException?.Message ?? e.Message;

            var failure = new ServerFailure
            {
                Message = $"Delete notification failed because: {detailedMessage}",
            };
            return Either<Failure, Unit>.Left(failure);
        }
    }

    public async Task<Either<Failure, Unit>> UpdateNotification(NotificationEntity entity)
    {
        try
        {
            context.Update(entity);
            await context.SaveChangesAsync();

            return new Unit();
        }
        catch (Exception e)
        {
            var detailedMessage = e.InnerException?.Message ?? e.Message;

            var failure = new ServerFailure
            {
                Message = $"Update notification failed because: {detailedMessage}",
            };

            return Either<Failure, Unit>.Left(failure);
        }
    }

    public async Task<Either<Failure, List<NotificationEntity>>> GetUserNotifications(string userid)
    {
        try
        {
            return await context
                .Notifications
                .Where(n => n.CreatedBy == userid)
                .ToListAsync();
        }
        catch (Exception e)
        {
            var detailedMessage = e.InnerException?.Message ?? e.Message;

            var failure = new ServerFailure
            {
                Message = $"Update notification failed because: {detailedMessage}",
            };
            return Either<Failure, List<NotificationEntity>>.Left(failure);
        }
    }

    public async Task<Option<bool>> VerifyNotificationIsFromUser(string notificationId, string userid)
    {
        var notification = await context.Notifications.FindAsync(notificationId);
        
        return notification != null ? notification.CreatedBy == userid : Option<bool>.None;
    }
}