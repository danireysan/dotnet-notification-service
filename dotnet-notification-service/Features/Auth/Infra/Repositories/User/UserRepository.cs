using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories.User;

public class UserRepository(UserContext context) : IUserRepository
{
    public Task<Either<Failure, EmailAddress>> EnsureMailIsUnique(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<Either<Failure, Unit>> Add(UserEntity user)
    {
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Either<Failure, Unit>.Right(new Unit());
        }
        catch (Exception e)
        {
            var failure = new ServerFailure
            {
                Message = "Unable to add user" + e.Message,
            };

            return Either<Failure, Unit>.Left(failure);
        }
    }
}