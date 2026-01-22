
using dotnet_notification_service.Domain.Entities;

using dotnet_notification_service.Features.Auth.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Application
{
    public class CreateUserUsecase : ICreateUserUsecase
    {
        public Task<Either<Failure, UserAccessedEntity>> CallAsync(CreateUserRequestEntity @params)
        {
            throw new NotImplementedException();
        }
    }
}