using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories;

public class CustomPasswordHasher : ICustomPasswordHasher
{
    public Task<Either<Failure, PasswordHash>> HashPassword(string email, string password)
    {
        throw new NotImplementedException();
    }

    
}