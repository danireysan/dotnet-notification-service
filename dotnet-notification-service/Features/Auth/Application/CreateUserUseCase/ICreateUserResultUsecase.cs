using dotnet_notification_service.Core.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using dotnet_notification_service.Features.Auth.Domain.Entities;

namespace dotnet_notification_service.Features.Auth.Application
{
    public interface ICreateUserUseCase : IUseCase<CreateUserResult,CreateUserCommand>
    {
    }

}