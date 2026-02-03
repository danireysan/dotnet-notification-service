using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API;
using dotnet_notification_service.Features.Notifications.Application;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using Funcky.Monads;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationsControllerTest
{
    private readonly Mock<ICreateNotificationUseCase> _createNotificationUseCase = new();
    private readonly CreateNotificationCommand _createNotificationCommand = new();

    [Fact]
    public async Task CreateNotification_ShouldReturnCreated_WhenUseCaseSucceeds()
    {


        _createNotificationUseCase
            .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationCommand>()))
            .ReturnsAsync(Either<Failure, CreateNotificationResult>.Right(new CreateNotificationResult()));
        
        var controller = new NotificationsController(_createNotificationUseCase.Object);
        //? Act
        var result = await controller.CreateNotification(_createNotificationCommand);
        // perform expected result in UseCase
        // Use the UseCase in the controller
        //? Assert
        // what should you assert? that you got the expected HTTP result
        _createNotificationUseCase.Verify();
        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task CreateNotification_ShouldReturnBadRequest_WhenUseCaseFails()
    {
        //? Arrange
        var failure = new ServerFailure
        {
            Message = "Failure"
        };
        
        _createNotificationUseCase
            .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationCommand>()))
            .ReturnsAsync(Either<Failure, CreateNotificationResult>.Left(failure));
        //? Act
        var controller = new NotificationsController(_createNotificationUseCase.Object);        
        //? Assert
        var result = await controller.CreateNotification(_createNotificationCommand);
        _createNotificationUseCase.Verify();
        Assert.IsType<BadRequestResult>(result);
    }

    // [Fact]
    // public async Task CreateNotification_ShouldReturnUnauthorized_WhenUserIsUnauthorized()
    // {
    //     //? Arrange
    //     var failure = new Failure
    //     {
    //         Message = "Something went wrong"
    //     };
    //     _createNotificationUseCase
    //         .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationCommand>()))
    //         .ReturnsAsync(Either<Failure, CreateNotificationResult>.Left(failure));
    //     //? Act
    //     var controller = new NotificationsController(_createNotificationUseCase.Object);        
    //     //? Assert
    //     var result = await controller.CreateNotification(_createNotificationCommand);
    //     _createNotificationUseCase.Verify();
    //     var badRequestResult = Assert.IsType<BadRequestResult>(result);
    //     var badRequest = badRequestResult.StatusCode;
    //     Assert.Equal(401, badRequest);
    // }

    [Fact]
    public async Task UpdateNotification_ShouldReturnOk_WhenUseCaseSucceeds()
    {
        //? Arrange
        //? Act
        //? Assert
    }

    [Fact]
    public async Task UpdateNotification_ShouldReturnBadRequest_WhenUseCaseFails()
    {
        //? Arrange
        //? Act
        //? Assert
    }

    [Fact]
    public async Task DeleteNotification_ShouldReturnNoContent_WhenUseCaseSucceeds()
    {
        //? Arrange
        //? Act
        //? Assert
    }

    [Fact]
    public async Task DeleteNotification_ShouldReturnBadRequest_WhenUseCaseFails()
    {
        //? Arrange
        //? Act
        //? Assert
    }

    [Fact]
    public async Task GetNotifications_ShouldReturnOk_WhenUseCaseSucceeds()
    {
        //? Arrange
        //? Act
        //? Assert
    }

    [Fact]
    public async Task GetNotifications_ShouldReturnBadRequest_WhenUseCaseFails()
    {
        //? Arrange
        //? Act
        //? Assert
    }
    
    

}

