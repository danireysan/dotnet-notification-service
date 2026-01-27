using dotnet_notification_service.Features.Auth.Infra.Repositories;
using JetBrains.Annotations;

namespace dotnet_notification_service.Tests.Features.Auth.Infra.Repositories;

[TestSubject(typeof(TokenRepository))]
public class TokenRepositoryTest
{

    [Fact]
    public async Task Generate_returns_token()
    {
        
    }
    
    [Fact]
    public async Task Generate_ShouldReturn_FailureIfTokenIsNull()
    {

    }
}