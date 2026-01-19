using dotnet_notification_service.Core.Usecases;
using dotnet_notification_service.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Domain.Repositories
{
    public interface IAuthRepository
    {
        Task<Either<Failure, CreateUserResultEntity>> CreateUserAsync(CreateUserRequestEntity createUserRequestEntity);
        Task<Either<Failure, CreateUserResultEntity>> LoginUserAsync(CreateUserRequestEntity createUserRequestEntity);

    }
}