
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Infra.Repositories.User;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace dotnet_notification_service.Tests.Features.Auth.Infra.Repositories;

[TestSubject(typeof(UserRepository))]
public class UserRepositoryTest : IAsyncLifetime
{
    // Pass the image directly to the constructor to solve CS0618
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16")
        .Build();

    private UserContext _context;
    private UserRepository _repository;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<UserContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;

        _context = new UserContext(options);
        
        await _context.Database.EnsureCreatedAsync();

        _repository = new UserRepository();
    }

    [Fact]
    public async Task CreateUser_ShouldReturnUnit_WhenDatabaseSavesEntity()
    {
        //? Arrange
        var user = UserEntity.Stub();
        //? Act
        var result = _repository.Add(user);
        //? Assert
    }
    
    [Fact]
    public async Task EnsureMailIsUnique_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        
    }
    
    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}