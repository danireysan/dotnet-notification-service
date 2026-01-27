using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;
using Microsoft.AspNetCore.Identity;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories;


using EitherHashOrFailure = Either<Failure, PasswordHash>;
using EitherBoolOrFailure = Either<Failure, bool>;
public class PasswordHashingService(PasswordHasher<UserId> passwordHasher) : ICustomPasswordHasher
{
    public Task<EitherHashOrFailure> HashPassword(UserId userId, string password)
    {
        try
        {
            var hash = passwordHasher.HashPassword(userId, password);
            var hashedPassword = new PasswordHash(hash);

            return Task.FromResult(EitherHashOrFailure.Right(hashedPassword));
        }
        catch (Exception e)
        {
            var failure = new ServerFailure
            {
                Message = "Failed to hash password because: " + e.Message,
            };
            return Task.FromResult(EitherHashOrFailure.Left(failure));
        }
    }

    public Task<EitherBoolOrFailure> VerifyPassword(UserEntity user, string password)
    {
        try
        {
            var result = passwordHasher.VerifyHashedPassword(user.Id, user.PasswordHash.Value, password);

            if (result == PasswordVerificationResult.Success)
            {
                return Task.FromResult(EitherBoolOrFailure.Right(true));
            }

            var failure = new UnauthorizedFailure
            {
                Message = "Failed to hash password because: " + result.ToString(),
            };
            
            return Task.FromResult(EitherBoolOrFailure.Left(failure));
            
            
        }
        catch (Exception e)
        {
            var serverFailure = new ServerFailure
            {
                Message = e.Message,
            };
            return Task.FromResult(EitherBoolOrFailure.Left(serverFailure));
        }
    }
}