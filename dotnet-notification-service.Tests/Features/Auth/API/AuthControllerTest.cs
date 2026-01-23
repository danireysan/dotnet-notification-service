using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.API.Controllers;
using dotnet_notification_service.Features.Auth.API.DTOS;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using Funcky.Monads;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dotnet_notification_service.Tests.Features.Auth.API
{
    public class AuthControllerTests

    {
        private static readonly Mock<ICreateUserUseCase> MockUseCase = new();

        private static readonly CreateUserRequest MockCreateUserRequest = new(
            "some@mail.com",
            "somePassword"
        );

        [Fact]
        public async Task CreateUser_ShouldReturnCreated_WhenUseCaseSucceeds()
        {
            // ? ARRANGE
            // expected result
            var expectedResult = new CreateUserResult(
                "someToken",
                "someUserId"
            );

            var expectedResponse = new CreateUserResponse(
                expectedResult.Token
            );

            // perform expected result in UseCase
            MockUseCase
                .Setup(uc => uc.CallAsync(It.IsAny<CreateUserCommand>()))
                .ReturnsAsync(Either<Failure, CreateUserResult>.Right(expectedResult));


            // Use the UseCase in the controller
            var controller = new AuthController(MockUseCase.Object);

            // ? ACT
            // Just call the endpoint
            var result = await controller.CreateUser(MockCreateUserRequest);

            // ? ASSERT
            // what should you assert? that you got the expected HTTP result


            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            var createdValue = Assert.IsType<CreateUserResponse>(createdResult.Value);

            Assert.Equal(expectedResponse.Token, createdValue.Token);
        }


        [Fact]
        public async Task CreateUser_ShouldReturnBadRequest_WhenUseCaseFails()
        {
            //? Arrange
            var failure = new Failure
            {
                Message = "someError"
            };

            // perform expected result in UseCase
            MockUseCase
                .Setup(uc => uc.CallAsync(It.IsAny<CreateUserCommand>()))
                .ReturnsAsync(Either<Failure, CreateUserResult>.Left(failure));
            // Use the UseCase in the controller
            var controller = new AuthController(MockUseCase.Object);
            //? Act
            var result = await controller.CreateUser(MockCreateUserRequest);
            //? Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnedFailure = Assert.IsType<Failure>(badRequestResult.Value);
            Assert.Equal(failure.Message, returnedFailure.Message);
        }
    }
}