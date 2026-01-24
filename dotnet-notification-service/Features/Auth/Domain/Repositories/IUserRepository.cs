using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Domain.Repositories;

public interface IUserRepository
{
    Task<Either<Failure, bool>> IsMailUnique(string email);
}