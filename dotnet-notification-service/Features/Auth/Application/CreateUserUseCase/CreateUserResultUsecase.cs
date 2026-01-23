using dotnet_notification_service.Core.Domain.Entities;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Application.CreateUserUseCase
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        public Task<Either<Failure, CreateUserResult>> CallAsync(CreateUserCommand @params)
        {
            throw new NotImplementedException();
        }
    }
}