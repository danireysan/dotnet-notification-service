using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky.Monads;
using NUlid;

namespace dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;

public class CreateNotificationUseCase
    : ICreateNotificationUseCase
{
    
    private readonly IReadOnlyDictionary<NotificationChannel, INotificationSender> _senders;
    private readonly INotificationRepository _repository;
    
    public CreateNotificationUseCase(IEnumerable<INotificationSender> senders, INotificationRepository repository)
    {
        _senders = senders.ToDictionary(s => s.Channel);
        _repository = repository;
    }

    public async Task<Either<Failure, CreateNotificationResult>> CallAsync(CreateNotificationCommand @params)
    {
        var ulid = Ulid.NewUlid();
        var data = @params.Data;
        var entity = new NotificationEntity(ulid, data.Title, data.Content, data.Recipient, @params.UserId, data.Channel);
        var result = await _repository.SaveNotification(entity);

        
        return from _ in result
            select new CreateNotificationResult();
    }
}