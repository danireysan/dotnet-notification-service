using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Domain.Repositories;

public interface IUserRepository
{
    Task<Either<Failure, EmailAddress>> EnsureMailIsUnique(string? email);
    Task<Either<Failure, Unit>> Add(UserEntity user);
}