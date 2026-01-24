using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;
using Moq;

namespace dotnet_notification_service.Tests.Features.Auth.Application;

public class CreateUserUseCaseTest
{   
    
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<ICustomPassWordHasher> _hasherMock;
    private readonly Mock<ITokenRepository> _tokenMock;
    private readonly CreateUserUseCase _useCase;

    public CreateUserUseCaseTest()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<ICustomPassWordHasher>();
        _tokenMock = new Mock<ITokenRepository>();
        
        // Inject both into the UseCase
        _useCase = new CreateUserUseCase(_userRepoMock.Object, _hasherMock.Object, _tokenMock.Object);
    }

    [Theory]
    [InlineData("mail")]
    [InlineData("mail@")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1431324")]
    public async Task CallAsync_ShouldReturnFailure_WhenEmailIsInvalid(string? mail)
    {
        //? Arrange
        var createUserCommand = new CreateUserCommand(mail ?? "", "");
        //? Act
        var result = await _useCase.CallAsync(createUserCommand);
        //? Assert
        result.Switch(
            left: left =>
            {
                Assert.IsType<UnprocessableEntityFailure>(left);
            },
            right: _ => { Assert.Fail("Expected a failure"); }
        );

    }

    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        //? Arrange
        var createUserCommand = new CreateUserCommand("user@mail.com", "mail");
        var expectedFailure = new ConflictFailure
        {
            Message = "Email already exists.",
        };
        _userRepoMock
            .Setup(repo => repo.IsMailUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, bool>.Left(expectedFailure));
        
        //? Act
        var result = await _useCase.CallAsync(createUserCommand);
        //? Assert
        result.Switch(
            left: left =>
            {
                Assert.IsType<ConflictFailure>(left);
            },
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }
        
    

    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenHashFails()
    {
        //? Arrange
        //? Act
        //? Assert
    }

    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenCreateUserFails()
    {
        
    }

    [Fact]
    public async Task CallAsync_ShouldReturnUserCreatedResult_WhenPasswordIsHashedAndUserIdIsCreated()
    {
        
        
    }


}