

using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using dotnet_notification_service.Features.Auth.API;
using dotnet_notification_service.Features.Auth.Domain.Entities;

using Funcky.Monads;
using dotnet_notification_service.Domain.Entities;
using dotnet_notification_service.Features.Auth.Application;


namespace dotnet_notification_service.Tests.Features.Auth.API
{
    public class AuthControllerTests

    {
        readonly CreateUserRequestEntity request = new()
        {
            Email = "test@example.com",
            Password = "password123"
        };

        [Fact]
        public async Task CreateUser_ShouldReturnCreated_WhenUsecaseSucceeds()
        {
            // Arrange
            var mockUsecase = new Mock<ICreateUserUsecase>();

            var expectedResult = new CreateUserResultEntity
            {
                UserId = "12345",
                Token = "some-jwt-token"
            };

            mockUsecase
                .Setup(uc => uc.CallAsync(It.IsAny<CreateUserRequestEntity>()))
                .ReturnsAsync(Either<Failure, CreateUserResultEntity>.Right(expectedResult));

            var controller = new AuthController(mockUsecase.Object);

            // Act
            var result = await controller.CreateUser(request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(expectedResult, createdResult.Value);
        }


        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUsecaseFails()
        {
            // Arrange
            var failedMockUsecase = new Mock<ICreateUserUsecase>();
            var failure = new Failure
            {
                Message = "User creation failed"
            };

            failedMockUsecase.Setup(uc => uc.CallAsync(It.IsAny<CreateUserRequestEntity>()))
                .ReturnsAsync(Either<Failure, CreateUserResultEntity>.Left(failure));

            var controller = new AuthController(failedMockUsecase.Object);

            // Act
            var result = await controller.CreateUser(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);


        }
    }


}