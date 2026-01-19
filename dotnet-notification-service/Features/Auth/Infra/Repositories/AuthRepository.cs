namespace dotnet_notification_service.Features.Auth.Infra.Repositories
{
    using dotnet_notification_service.Domain.Entities;
    using dotnet_notification_service.Features.Auth.Domain.Entities;
    using dotnet_notification_service.Features.Auth.Domain.Repositories;
    using Funcky.Monads;

    public class AuthRepository : IAuthRepository
    {
        public Task<Either<Failure, CreateUserResultEntity>> CreateUserAsync(CreateUserRequestEntity createUserRequestEntity)
        {
            throw new NotImplementedException();
        }

        public Task<Either<Failure, CreateUserResultEntity>> LoginUserAsync(CreateUserRequestEntity createUserRequestEntity)
        {
            throw new NotImplementedException();
        }
    }
}