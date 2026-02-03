using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky;
using Funcky.Monads;
using Microsoft.EntityFrameworkCore;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories.User;

public class UserRepository(UserContext context) : IUserRepository
{
    private EmailAddress _getOrElse;

    public async Task<Either<Failure, EmailAddress>> EnsureMailIsUnique(string email)
    {
        try
        {
            var mail = EmailAddress.FromPersistence(email);
            var exists = await context.Users.AnyAsync(
                e => e.Email == mail
                );
            var failure = new ConflictFailure
            {
                Message = $"It seems that {email} already exists"
            };
            if (exists)
                return Either<Failure, EmailAddress>.Left(failure);
            
            return mail;
        }
        catch (Exception e)
        {

            var failure = new ServerFailure
            {
                Message = "EnsureMailIsUnique failed because:  " + e.Message, 
            };
            return Either<Failure, EmailAddress>.Left(failure);
        }
    }

    public async Task<Either<Failure, Unit>> Add(UserEntity user)
    {
        try
        {
            var isUnique = await EnsureMailIsUnique(user.Email.Value);
            var left = isUnique.LeftOrNone().ToNullable();
            if (left != null)
                return Either<Failure, Unit>.Left(left);
            
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return new Unit();
        }
        catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException { SqlState: "23505" })
        {
            return Either<Failure, Unit>.Left(new ConflictFailure
            {
                Message = $"It seems that this {user.Email.Value} already exists"
            });
        }
        catch (Exception e)
        {
            return Either<Failure, Unit>.Left(new ServerFailure
            {
                Message = "Unable to add user: " + e.Message,
            });
        }
    }
}