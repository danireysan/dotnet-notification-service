using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;
using Microsoft.EntityFrameworkCore;
using NUlid;

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
            var isValidId = Ulid.TryParse(id, out var ulid);
            if (!isValidId)
            {
                var failure = new NotFoundFailure { Message = "This notification doesn't exist." };
                return Either<Failure, Unit>.Left(failure);
            }

            var entity = await context.Notifications.FirstOrDefaultAsync(n => n.NotificationId == ulid);
            if (entity == null)
            {
                var failure = new NotFoundFailure { Message = "This notification doesn't exist." };
                return Either<Failure, Unit>.Left(failure);
            }

            context.Remove(entity);
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

    public async Task<Either<Failure, NotificationEntity>> GetNotificationById(string id)
    {
        var failure = new NotFoundFailure { Message = "This notification doesn't exist." };
        if (!Ulid.TryParse(id, out var ulid))
        {
            return Either<Failure, NotificationEntity>.Left(failure);
        }


        var notification = await context.Notifications.AsNoTracking()
            .FirstOrDefaultAsync(n => n.NotificationId == ulid);

        return notification ?? Either<Failure, NotificationEntity>.Left(failure);
    }

    public Either<Failure, Unit> IsNotificationFromUser(NotificationEntity entity, string userId)
    {
        var forbiddenFailure = new ForbiddenFailure
        {
            Message = "This notification is not yours."
        };
        return entity.CreatedBy == userId ? new Unit() : Either<Failure, Unit>.Left(forbiddenFailure);
    }
}