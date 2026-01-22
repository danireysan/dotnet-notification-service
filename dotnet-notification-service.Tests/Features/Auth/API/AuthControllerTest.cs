

using Moq;
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
        private readonly CreateUserRequestEntity _request = new()
        {
            Email = "test@example.com",
            Password = "password123"
        };
        

        [Fact]
        public async Task CreateUser_ShouldReturnCreated_WhenUseCaseSucceeds()
        {
            // Arrange
            var mockUseCase = new Mock<ICreateUserUsecase>();

            var expectedResult = new UserAccessedEntity
            {
                UserId = "12345",
                Token = "some-jwt-token"
            };

            mockUseCase
                .Setup(uc => uc.CallAsync(It.IsAny<CreateUserRequestEntity>()))
                .ReturnsAsync(Either<Failure, UserAccessedEntity>.Right(expectedResult));

            var controller = new AuthController(mockUseCase.Object);

            // Act
            var result = await controller.CreateUser(_request);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal(expectedResult, createdResult.Value);
        }


        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUseCaseFails()
        {
            // Arrange
            var failedMockUseCase = new Mock<ICreateUserUsecase>();
            var failure = new Failure
            {
                Message = "User creation failed"
            };

            failedMockUseCase.Setup(uc => uc.CallAsync(It.IsAny<CreateUserRequestEntity>()))
                .ReturnsAsync(Either<Failure, UserAccessedEntity>.Left(failure));

            var controller = new AuthController(failedMockUseCase.Object);

            // Act
            var result = await controller.CreateUser(_request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);


        }
    }


}