using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky;
using Funcky.Monads;
using Moq;

namespace dotnet_notification_service.Tests.Features.Auth.Application;

public class CreateUserUseCaseTest
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<ICustomPasswordHasher> _hasherMock;
    private readonly Mock<ITokenRepository> _tokenMock;
    private readonly CreateUserUseCase _useCase;

    public CreateUserUseCaseTest()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<ICustomPasswordHasher>();
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
            left: left => { Assert.IsType<UnprocessableEntityFailure>(left); },
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
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Left(expectedFailure));

        //? Act
        var result = await _useCase.CallAsync(createUserCommand);
        //? Assert
        result.Switch(
            left: left => { Assert.IsType<ConflictFailure>(left); },
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }


    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenHashFails()
    {
        //? Arrange
        var createUserCommand = new CreateUserCommand("user@mail.com", "mail");
        var expectedFailure = new ServerFailure
        {
            Message = "Hash failure",
        };

        _userRepoMock
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Right(EmailAddress.MockCreate(createUserCommand.Email)));
        _hasherMock
            .Setup(repo => repo.HashPassword(createUserCommand.Email,  createUserCommand.Password))
            .ReturnsAsync(Either<Failure, PasswordHash>.Left(expectedFailure));
                

        
        //? Act
        var result = await _useCase.CallAsync(createUserCommand);
        //? Assert
        result.Switch(
            left: left => { Assert.IsType<ServerFailure>(left); },
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }

    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenCreateUserFails()
    {
        //? Arrange
        var createUserCommand = new CreateUserCommand("user@mail.com", "mail");
        var expectedFailure = new ServerFailure
        {
            Message = "DB failure",
        };
        var emailAddress = EmailAddress.MockCreate(createUserCommand.Email);
        var hashPassword = new PasswordHash(createUserCommand.Password);
        
        _userRepoMock
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Right(emailAddress));
        _hasherMock
            .Setup(repo => repo.HashPassword(createUserCommand.Email,  createUserCommand.Password))
            .ReturnsAsync(Either<Failure, PasswordHash>.Right(hashPassword));
        _userRepoMock
            .Setup(repo => repo.Add(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, Unit>.Left(expectedFailure));
        
        //? Act
        var result = await _useCase.CallAsync(createUserCommand);
        //? Assert
        result.Switch(
            left: left => { Assert.IsType<ServerFailure>(left); },
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }

    [Fact]
    public async Task CallAsync_ShouldReturnFailure_WhenTokenFails()
    {
        var createUserCommand = new CreateUserCommand("user@mail.com", "mail");
    }

    [Fact]
    public async Task CallAsync_ShouldReturnUserCreatedResult_WhenPasswordIsHashedAndUserIdIsCreated()
    {
        //? Arrange
        //? Act
        //? Assert
    }
}