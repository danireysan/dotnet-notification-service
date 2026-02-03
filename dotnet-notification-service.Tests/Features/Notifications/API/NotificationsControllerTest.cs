using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dotnet_notification_service.Core;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.Controllers;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using Funcky.Monads;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationsControllerTest


{
    
    [Fact]
    public async Task CreateNotification_ShouldReturnUnauthorized_WhenThereIsNoToken()
    {
        //? Arrange
        var sut = new NotificationsController(new Mock<ICreateNotificationUseCase>().Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() 
            }
        };

        var dto = new EmailNotificationDto("Hi", "Body", "test@example.com");

        //? Act
        var result = await sut.CreateNotification(dto);

        //? Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Theory]
    [ClassData(typeof(CommonFailureData))]
    public async Task CreateNotification_ShouldReturnBadRequest_WhenUseCaseFails(Type failureType, 
        int expectedStatusCode, 
        Type expectedResultType)
    {
        //? Arrange
        var failureInstance = (Failure)Activator.CreateInstance(failureType)! with 
        { 
            Message = "Domain Error" 
        };
        var useCase = new Mock<ICreateNotificationUseCase>();
        useCase
            .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationCommand>()))
            .ReturnsAsync(Either<Failure, CreateNotificationResult>.Left(failureInstance));
        

        var sut = new NotificationsController(useCase.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, "12345")
        ], "mock"));

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        //? Act
        var dto = new EmailNotificationDto("Hi", "Body", "test@example.com");
        var actionResult = await sut.CreateNotification(dto);
        //? Assert
        Assert.IsType(expectedResultType, actionResult);
            
        var objectResult = (ObjectResult)actionResult;
        Assert.Equal(expectedStatusCode, objectResult.StatusCode);
    
        var response = Assert.IsType<ErrorResponse>(objectResult.Value);
        Assert.Equal("Domain Error", response.Error);
    }
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
            .Setup(uc => uc.CallAsync(It.IsAny<CreateNotificationCommand>()))
            .ReturnsAsync(Either<Failure, CreateNotificationResult>.Right(new CreateNotificationResult()));
        

        var sut = new NotificationsController(useCase.Object);
        var user = new ClaimsPrincipal(new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, "12345")
        ], "mock"));

        sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        //? Act

        var result = await sut.CreateNotification(data.Dto);
        //? Assert
        Assert.NotNull(data.Dto);
        Assert.IsType<OkObjectResult>(result);
    }
    

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