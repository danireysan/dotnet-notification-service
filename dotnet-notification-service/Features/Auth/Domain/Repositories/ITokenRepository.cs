using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Domain.Repositories;

public interface ITokenRepository
{
    Task<Either<Failure, string>> Generate(UserEntity isAny);
}