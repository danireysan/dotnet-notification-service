using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using Microsoft.AspNetCore.Identity;
using NUlid;

namespace dotnet_notification_service.Tests.Features.Auth.Infra.Repositories;

public class PasswordHashingServiceTest
{
    private readonly UserId _defaultUserId = UserId.New();
    private readonly EmailAddress _defaultEmailAddress = EmailAddress.FromPersistence("danireysan@gmail.com");
    private readonly PasswordHasher<UserId> _passwordHasher = new();
    private readonly PasswordHashingService _passwordHashingService;

    public PasswordHashingServiceTest()
    {
        _passwordHashingService = new PasswordHashingService(_passwordHasher);
    }


    [Theory]
    [InlineData("password")]
    [InlineData("asdfafs")]
    [InlineData("sadf1234")]
    public async Task HashPassword_HashShouldNotBeTheSameAsPassword(string password)
    {
        //? Act
        var result = await _passwordHashingService.HashPassword(_defaultUserId, password);

        //? Assert
        result.Switch(
            left: _ => Assert.Fail("Expected exception"),
            right: hash => { Assert.NotEqual(hash.Value, password); }
        );
    }


    [Fact]
    public async Task HashPassword_ShouldReturnDifferentHash_WhenPasswordIsTheSame()
    {
        //? Arrange
        const string password = "password";
        var userId1 = _defaultUserId;
        var userId2 = _defaultUserId;
        //? Act
        var hash1 = (await _passwordHashingService.HashPassword(_defaultUserId, password))
            .GetOrElse(fallback: failure => throw new Exception("Expected hash"));
        var hash2 = (await _passwordHashingService.HashPassword(_defaultUserId, password))
            .GetOrElse(failure => throw new Exception("Expected hash"));
        //? Assert
        Assert.NotEqual(hash1, hash2);
    }


    [Theory]
    [InlineData("password")]
    [InlineData("asdfafs")]
    [InlineData("sadf1234")]
    public async Task VerifyPassword_ShouldReturnTrue_WhenPasswordIsCorrect(string password)
    {
        //? Arrange
        var hashEither = await _passwordHashingService.HashPassword(_defaultUserId, password);

        var hash = hashEither.GetOrElse(fallback: _ => throw new Exception("Expected hash"));

        var userEntity = UserEntity.Create(_defaultUserId, _defaultEmailAddress, hash);

        //? Act
        var verifyResult = await _passwordHashingService.VerifyPassword(userEntity, password);


        // Assert
        verifyResult.Switch(
            left: _ => Assert.Fail("Expected match"),
            right: Assert.True
        );
    }

    [Fact]
    public async Task VerifyPassword_ShouldReturnFailure_WhenPasswordIsNotCorrect()
    {
        var failure = new UnauthorizedFailure
        {
            Message = "Failed to hash password because: ",
        };
        //? Arrange
        const string password = "password";
        var hashEither = await _passwordHashingService.HashPassword(_defaultUserId, password);

        var hash = hashEither.GetOrElse(fallback: _ => throw new Exception("Expected hash"));

        var userEntity = UserEntity.Create(_defaultUserId, _defaultEmailAddress, hash);

        //? Act
        var verifyResult = await _passwordHashingService.VerifyPassword(userEntity, "otherPassword");
        //? Assert
        verifyResult.Switch(
            left: verifyFailure =>  Assert.IsType<UnauthorizedFailure>(verifyFailure),
            right: _ => Assert.Fail("Expected failure")
            );
    }
}