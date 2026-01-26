using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using Funcky.Monads;


namespace dotnet_notification_service.Features.Auth.Domain.Repositories;

public interface ICustomPasswordHasher
{
    Task<Either<Failure, PasswordHash>> HashPassword(string email, string password);
}