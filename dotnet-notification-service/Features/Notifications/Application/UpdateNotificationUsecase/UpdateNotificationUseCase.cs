using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;

public class UpdateNotificationUseCase : IUpdateNotificationUseCase
{
    public Task<Either<Failure, UpdateNotificationResult>> CallAsync(UpdateNotificationCommand @params)
    {
        // Minimal implementation: return success. Replace with real logic later.
        return Task.FromResult<Either<Failure, UpdateNotificationResult>>(new UpdateNotificationResult());
    }
}
