
using dotnet_notification_service.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Domain.Repositories
{
    public interface IAuthRepository
    {
        Task<Either<Failure, UserAccessedEntity>> CreateUserAsync(CreateUserRequestEntity createUserRequestEntity);
        Task<Either<Failure, UserAccessedEntity>> LoginUserAsync(CreateUserRequestEntity createUserRequestEntity);

    }
}