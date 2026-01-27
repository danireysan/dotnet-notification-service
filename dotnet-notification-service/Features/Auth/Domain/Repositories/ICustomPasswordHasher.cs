using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using Funcky.Monads;


namespace dotnet_notification_service.Features.Auth.Domain.Repositories;

public interface ICustomPasswordHasher
{
    Task<Either<Failure, PasswordHash>> HashPassword(UserId userId, string password);
    Task<Either<Failure, bool>>  VerifyPassword(UserEntity user, string password);
 }