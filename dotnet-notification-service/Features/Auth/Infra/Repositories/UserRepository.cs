using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories;

public class UserRepository : IUserRepository
{
    public Task<Either<Failure, bool>> IsMailUnique(string email)
    {
        throw new NotImplementedException();
    }
}