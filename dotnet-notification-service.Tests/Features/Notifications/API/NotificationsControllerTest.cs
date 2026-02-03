using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API;
using dotnet_notification_service.Features.Notifications.API.Controllers;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using Funcky.Monads;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationsControllerTest
{



   
    
    
    public static TheoryData<NotificationTestData> AllNotificationTypes =>
    [
        new(new EmailNotificationDto("Hi", "Body", "test@example.com")),
        new(new SmsNotificationDto("Alert", "Body", "1234567890")),
        new(new PushNotificationDto("Ping", "Body", "device-token-123"))
    ];

    [Theory, MemberData(nameof(AllNotificationTypes))]
    public async Task CreateNotification_ReturnsOk_ForAllDtoTypes(NotificationTestData data)
    {
        //? Arrange 
        var useCase = new Mock<ICreateNotificationUseCase>();
        useCase
            .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationDto>()))
            .ReturnsAsync(Either<Failure, CreateNotificationResult>.Right(new CreateNotificationResult()));
        
        var sut = new NotificationsController(useCase.Object);
        //? Act
        
        var result = await sut.CreateNotification(data.Dto);
        //? Assert
        Assert.NotNull(data.Dto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CreateNotification_ShouldReturnBadRequest_WhenUseCaseFails()
    {
        // //? Arrange
        // var failure = new ServerFailure
        // {
        //     Message = "Failure"
        // };
        //
        // _createNotificationUseCase
        //     .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationCommand>()))
        //     .ReturnsAsync(Either<Failure, CreateNotificationResult>.Left(failure));
        // //? Act
        // var controller = new NotificationsController(_createNotificationUseCase.Object);        
        // //? Assert
        // var result = await controller.CreateNotification(_createNotificationCommand);
        // _createNotificationUseCase.Verify();
        // Assert.IsType<BadRequestResult>(result);
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

