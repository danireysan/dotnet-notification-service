using System.Diagnostics;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.API.Controllers;
using dotnet_notification_service.Features.Auth.API.DTOS;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using Funcky.Monads;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace dotnet_notification_service.Tests.Features.Auth.API;

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


        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdValue = Assert.IsType<CreateUserResponse>(createdResult.Value);

        Assert.Equal(expectedResponse.Token, createdValue.Token);
    }


    [Theory]
    [InlineData(typeof(UnauthorizedFailure), 401, typeof(UnauthorizedObjectResult))]
    [InlineData(typeof(UnprocessableEntityFailure), 422, typeof(UnprocessableEntityObjectResult))]
    [InlineData(typeof(ConflictFailure), 409, typeof(ConflictObjectResult))]
    [InlineData(typeof(ServerFailure), 500, typeof(ObjectResult))]
    public async Task CreateUser_ShouldMapFailuresToCorrectStatusCodes(
        Type failureType, 
        int expectedStatusCode, 
        Type expectedResultType)
    {
        //? Arrange
        var failureInstance = (Failure)Activator.CreateInstance(failureType)! with 
        { 
            Message = "Domain Error" 
        };

        MockUseCase
            .Setup(uc => uc.CallAsync(It.IsAny<CreateUserCommand>()))
            .ReturnsAsync(Either<Failure, CreateUserResult>.Left(failureInstance));
            
        var controller = new AuthController(MockUseCase.Object);

        //? Act
        var actionResult = await controller.CreateUser(MockCreateUserRequest);

        //? Assert

        Assert.IsType(expectedResultType, actionResult.Result);
            
        var objectResult = (ObjectResult)actionResult.Result!;
        Assert.Equal(expectedStatusCode, objectResult.StatusCode);
    
        var response = Assert.IsType<ErrorResponse>(objectResult.Value);
        Assert.Equal("Domain Error", response.Error);
    }
}