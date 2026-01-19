using dotnet_notification_service.Core.Usecases;
using dotnet_notification_service.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Usecases
{
    public class CreateUserResultUsecase : IUseCase<CreateUserResultUsecase, CreateUserRequestEntity>
    {
        public Task<Either<Failure, CreateUserResultUsecase>> CallAsync(CreateUserRequestEntity @params)
        {
            throw new NotImplementedException();
        }
    }
}