using dotnet_notification_service.Core.Application;

namespace dotnet_notification_service.Features.Auth.Application.CreateUserUseCase
{
    public interface ICreateUserUseCase : IUseCase<CreateUserResult,CreateUserCommand>
    {
    }

}