using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Infra.Repositories.User;
using Funcky;
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

        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnUnit_WhenDatabaseSavesEntity()
    {
        //? Arrange
        var user = UserEntity.Stub();
        //? Act
        var result = await _repository.Add(user);
        //? Assert
        result.Switch(
            left: failure => Assert.Fail("Expected user creation "),
            right: unit2 => { Assert.IsType<Unit>(unit2); }
        );

        // verify the user is actually saved
        var dbUser = await _context.Users.FindAsync(user.Id);
        Assert.NotNull(dbUser);
        Assert.Equal(user.Id, dbUser.Id);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        //? Arrange
        var user = UserEntity.Stub();
        //? Act
        await _repository.Add(user);
        var result = await _repository.Add(user);
        //? Assert
        result.Switch(
            left: failure => { Assert.IsType<ConflictFailure>(failure); },
            right: unit => Assert.Fail("Expected email address already exists")
        );
    }

    [Fact]
    public async Task EnsureMailIsUnique_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        //? Arrange
        var user = UserEntity.Stub();


        //? Act
        await _repository.Add(user);
        var result = await _repository.EnsureMailIsUnique(user.Email.Value);

        //? Assert
        result.Switch(
            left: failure => { Assert.IsType<ConflictFailure>(failure); },
            right: _ => Assert.Fail("Expected email address already exists")
        );
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}