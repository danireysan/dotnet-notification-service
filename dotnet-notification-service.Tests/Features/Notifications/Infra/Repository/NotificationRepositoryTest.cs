using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Infra.Repository;
using Funcky;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NUlid;
using Testcontainers.PostgreSql;

namespace dotnet_notification_service.Tests.Features.Notifications.Infra.Repository;

[TestSubject(typeof(NotificationRepository))]
public class NotificationRepositoryTest : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16")
        .Build();

    private NotificationContext? _context;
    private NotificationRepository? _repository;
    private readonly NotificationEntity _notification;


    public NotificationRepositoryTest()
    {
        var ulid = new Ulid();
        _notification = new NotificationEntity(ulid,"Title", "Content", "Recipient", "CreatedBy", NotificationChannel.Push, null);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<NotificationContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;

        _context = new NotificationContext(options);

        await _context.Database.EnsureCreatedAsync();

        _repository = new NotificationRepository(_context);
    }
    [Fact]
    public async Task CreateNotification_ShouldReturnUnit_WhenDatabaseSavesEntity()
    {
        //? Act
        var result = await _repository!.SaveNotification(_notification);
        //? Assert
        result.Switch(
            left: failure => Assert.Fail("Expected save entity"),
            right: unit => Assert.IsType<Unit>(unit)
        );
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}