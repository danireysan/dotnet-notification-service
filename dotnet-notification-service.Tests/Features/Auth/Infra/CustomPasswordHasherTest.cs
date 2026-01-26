using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using Microsoft.AspNetCore.Identity;
using NUlid;

namespace dotnet_notification_service.Tests.Features.Auth.Infra;

public class CustomPasswordHasherTest
{
    [Theory]
    [InlineData("password")]
    [InlineData("asdfafs")]
    [InlineData("sadf1234")]
    public async Task HashPassword_HashShouldNotBeTheSameAsPassword(string password)
    {
        // var passwordHasher = new PasswordHasher<UserEntity>();
        // var sut = new CustomPasswordHasher(passwordHasher);
        //
        //
        // var id = Ulid.NewUlid();
        // var mail = "danireysan@gmail.com";
        // var passwordHash = new PasswordHash(password);
        // var userEntity = UserEntity.CreateWithHash(id, mail, passwordHash);
        //     
        //
        // var result =  await sut.HashPassword(mail, password);
        //
        // result.Switch(
        //     left: _ => Assert.Fail("Expected exception"),
        //     right: hash =>
        //     {
        //         Assert.NotEqual(hash, passwordHash);
        //     }
        //     
        // );
        

    }
    // TODO:
    //Verify that a generated hash returns true when checked against the original password.
    // Verify that a hash returns false when checked against an incorrect password.
    //Verify that hashing the same password twice results in two different strings (Salting).
    
}