using dotnet_notification_service.Features.Auth.Infra.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories.User;
using JetBrains.Annotations;
using Testcontainers.PostgreSql;

namespace dotnet_notification_service.Tests.Features.Auth.Infra.Repositories;

[TestSubject(typeof(UserRepository))]
public class UserRepositoryTest : IAsyncLifetime
{
    private UserRepository _repository;
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }
    
    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void METHOD()
    {
        
    }

    

    
}