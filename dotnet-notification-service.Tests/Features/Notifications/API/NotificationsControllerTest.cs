using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dotnet_notification_service.Core;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.Controllers;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;
using Funcky.Monads;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationsControllerTest


{
    private readonly Mock<ICreateNotificationUseCase> _createUseCaseMock = new();
    private readonly Mock<IUpdateNotificationUseCase> _updateNotificationMock = new();

    [Fact]
    public async Task CreateNotification_ShouldReturnUnauthorized_WhenThereIsNoToken()
    {
        //? Arrange
        var sut = new NotificationsController(_createUseCaseMock.Object, _updateNotificationMock.Object)
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
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    
    

    
}