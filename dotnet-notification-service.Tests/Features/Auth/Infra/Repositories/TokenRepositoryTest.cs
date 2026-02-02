using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using Funcky.Monads;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace dotnet_notification_service.Tests.Features.Auth.Infra.Repositories;

[TestSubject(typeof(TokenRepository))]
public class TokenRepositoryTest
{
    [Fact]
    public async Task Generate_returns_JWT()
    {
        //? Arrange
        var user = UserEntity.Stub();
        
        var jwtOptions = new JwtOptions
        {
            Key = "S617szN2xVMFj37yhfx2qozeW321Jy61A2KX3qVHXS4=",
            Issuer = "test-issuer",
            Audience = "test-audience",
            ExpiresMinutes = 60
        };
        var options = Options.Create(jwtOptions);
        var repository = new TokenRepository(options);
        //?  Act
        var result = await repository.Generate(user);
        //? Assert
        result.Switch(
            left: _ => Assert.Fail("Expected token"),
            right: token =>
            {
                Assert.NotNull(token);
                // should have 3 parts when it's a valid token (header, payload, signature)
                var parts = token.Split('.');
                Assert.Equal(3, parts.Length);
            }
        );
    }
    
}