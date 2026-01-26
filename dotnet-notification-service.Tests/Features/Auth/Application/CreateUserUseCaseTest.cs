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

    // Extracted globals for reuse in tests
    private readonly CreateUserCommand _defaultCommand;
    private readonly EmailAddress _defaultEmailAddress;
    private readonly PasswordHash _defaultHashPassword;
    private readonly UserEntity _defaultUserEntity;

    public CreateUserUseCaseTest()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<ICustomPasswordHasher>();
        _tokenMock = new Mock<ITokenRepository>();

        // Inject both into the UseCase
        _useCase = new CreateUserUseCase(_userRepoMock.Object, _hasherMock.Object, _tokenMock.Object);

        // Initialize shared test data
        _defaultCommand = new CreateUserCommand("user@mail.com", "mail");
        _defaultEmailAddress = EmailAddress.FromPersistence(_defaultCommand.Email);
        _defaultHashPassword = new PasswordHash(_defaultCommand.Password);
        _defaultUserEntity = UserEntity.Create(_defaultEmailAddress, _defaultHashPassword);
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
        var createUserCommand = _defaultCommand;
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
        var createUserCommand = _defaultCommand;
        var expectedFailure = new ServerFailure
        {
            Message = "Hash failure",
        };

        _userRepoMock
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Right(EmailAddress.FromPersistence(createUserCommand.Email)));
        _hasherMock
            .Setup(repo => repo.HashPassword(It.IsAny<String>(),  createUserCommand.Password))
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
        // use the extracted defaults instead of local variables
        var createUserCommand = _defaultCommand;
        
        var userEntity = _defaultUserEntity;
        
        var expectedFailure = new ServerFailure
        {
            Message = "DB failure",
        };
        
        _userRepoMock
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Right(_defaultEmailAddress));
        _hasherMock
            .Setup(repo => repo.HashPassword(createUserCommand.Email,createUserCommand.Password))
            .ReturnsAsync(Either<Failure, PasswordHash>.Right(_defaultHashPassword));
        _userRepoMock
            .Setup(repo => repo.Add(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, Unit>.Left(expectedFailure));
        _tokenMock
            .Setup(repo => repo.Generate(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, string>.Right("asdfasdf"));
        
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
        //? Arrange
        var createUserCommand = _defaultCommand;
        var expectedFailure = new ServerFailure
        {
            Message = "Token failure",
        };
        
        _userRepoMock
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Right(_defaultEmailAddress));
        _hasherMock
            .Setup(repo => repo.HashPassword(It.IsAny<String>(),  createUserCommand.Password))
            .ReturnsAsync(Either<Failure, PasswordHash>.Right(_defaultHashPassword));
        _userRepoMock
            .Setup(repo => repo.Add(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, Unit>.Right(new Unit()));
        _tokenMock
            .Setup(repo => repo.Generate(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, string>.Left(expectedFailure));
                
        
        //? Act
        var result = await _useCase.CallAsync(createUserCommand);
        //? Assert
        result.Switch(
            left: left => { Assert.IsType<ServerFailure>(left); },
            right: _ => { Assert.Fail("Expected a failure"); }
        );
    }

    [Fact]
    public async Task CallAsync_ShouldReturnUserCreatedResult_WhenPasswordIsHashedAndUserIdIsCreated()
    {
        //? Arrange
        var createUserCommand = _defaultCommand;
        var expectedResult = new CreateUserResult(
            "someToken", "someId"
        );
        
       
        
        _userRepoMock
            .Setup(repo => repo.EnsureMailIsUnique(createUserCommand.Email))
            .ReturnsAsync(Either<Failure, EmailAddress>.Right(_defaultEmailAddress));
        _hasherMock
            .Setup(repo => repo.HashPassword(It.IsAny<String>(),  createUserCommand.Password))
            .ReturnsAsync(Either<Failure, PasswordHash>.Right(_defaultHashPassword));
        _userRepoMock
            .Setup(repo => repo.Add(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, Unit>.Right(new Unit()));
        _tokenMock
            .Setup(repo => repo.Generate(It.IsAny<UserEntity>()))
            .ReturnsAsync(Either<Failure, string>.Right(expectedResult.Token));
        //? Act
        var useCaseResult = await _useCase.CallAsync(createUserCommand);
        //? Assert
        useCaseResult.Switch(
            left: left => { Assert.Fail(left.Message); },
            right: result =>
            {
                var mockResult = Assert.IsType<CreateUserResult>(result);
                Assert.Equal(expectedResult.Token, mockResult.Token);
            }
        );
    }
}
