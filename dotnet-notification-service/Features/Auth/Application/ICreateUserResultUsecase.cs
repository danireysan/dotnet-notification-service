using dotnet_notification_service.Core.Usecases;
using dotnet_notification_service.Features.Auth.Domain.Entities;

namespace dotnet_notification_service.Features.Auth.Application
{
    public interface ICreateUserUsecase : IUseCase<UserAccessedEntity, CreateUserRequestEntity>
    {
    }

}