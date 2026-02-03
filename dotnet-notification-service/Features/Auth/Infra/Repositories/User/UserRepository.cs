using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories.User;

public class UserRepository : IUserRepository
{
    public Task<Either<Failure, EmailAddress>> EnsureMailIsUnique(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Either<Failure, Unit>> Add(UserEntity user)
    {
        throw new NotImplementedException();
    }
}