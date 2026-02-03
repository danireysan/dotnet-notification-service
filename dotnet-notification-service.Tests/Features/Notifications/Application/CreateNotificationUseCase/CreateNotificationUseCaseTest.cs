using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using JetBrains.Annotations;
using Moq;

namespace dotnet_notification_service.Tests.Features.Notifications.Application.CreateNotificationUseCase;

using Uc = dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;

[TestSubject(typeof(Uc.CreateNotificationUseCase))]
public class CreateNotificationUseCaseTest
{
    [Fact]
    public async Task CallAsync_ShouldReturnLeft_WhenNotificationIsNotSaved()
    {
        //? Arrange
        var sut = new Uc.CreateNotificationUseCase();
        //? Act
        var result = await sut.CallAsync(It.IsAny<CreateNotificationCommand>());
        //? Assert
        result.Switch(
            failure => Assert.True(true),
            right: _ => Assert.Fail("Expected Notification failure")
        );
    }

    [Fact]
    public async Task CallAsync_ShouldReturnRight_WhenNotificationIsSaved()
    {
        //? Arrange
        var sut = new Uc.CreateNotificationUseCase();

        //? Act
        var result = await sut.CallAsync(It.IsAny<CreateNotificationCommand>());
        //? Assert
        result.Switch(
            failure => Assert.Fail("Expected created notification"),
            right: _ => Assert.Fail("Expected Notification failure")
        );
    }
}