using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using Funcky;
using Funcky.Monads;
using JetBrains.Annotations;
using Moq;
using NUlid;

namespace dotnet_notification_service.Tests.Features.Notifications.Application.CreateNotificationUseCase;

using Uc = dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;

[TestSubject(typeof(Uc.CreateNotificationUseCase))]
public class CreateNotificationUseCaseTest
{
    private readonly Mock<INotificationRepository> _repository;
    private readonly Mock<INotificationSender> _sender;
    private readonly NotificationEntity _notification;

    public CreateNotificationUseCaseTest()
    {
        _repository = new Mock<INotificationRepository>();
        _sender = new Mock<INotificationSender>();
        var ulid = new Ulid();
        _notification = new NotificationEntity(ulid,"Title", "Content", "Recipient", "CreatedBy", NotificationChannel.Push);
    }

    [Fact]
    public async Task CallAsync_ShouldReturnLeft_WhenNotificationIsNotSaved()
    {
        // Arrange
        // repository returns a Failure (Left)
        var failure = new ServerFailure
        {
            Message = "Server error"
        };
        _repository
            .Setup(repo => repo.SaveNotification(It.IsAny<NotificationEntity>()))
            .ReturnsAsync(Either<Failure, Unit>.Left(failure));

        var sut = new Uc.CreateNotificationUseCase(_repository.Object, _sender.Object);

        var command = new CreateNotificationCommand(new EmailNotificationDto("Title", "Content", "test@example.com"), "someId");

        // Act
        var result = await sut.CallAsync(command);

        // Assert
        result.Switch(
            left => Assert.Same(failure, left),
            right: _ => Assert.Fail("Expected Notification failure")
        );
    }

    [Fact]
    public async Task CallAsync_ShouldReturnRight_WhenNotificationIsSaved()
    {
        // Arrange
        _repository
            .Setup(repo => repo.SaveNotification(It.IsAny<NotificationEntity>()))
            .ReturnsAsync(Either<Failure, Unit>.Right(new Unit()));

        var sut = new Uc.CreateNotificationUseCase(_repository.Object, _sender.Object);

        var command = new CreateNotificationCommand(new EmailNotificationDto("Title", "Content", "test@example.com"), "someId");

        // Act
        var result = await sut.CallAsync(command);

        // Assert
        result.Switch(
            left => Assert.Fail("Expected created notification"),
            right: created => Assert.NotNull(created)
        );
    }
}