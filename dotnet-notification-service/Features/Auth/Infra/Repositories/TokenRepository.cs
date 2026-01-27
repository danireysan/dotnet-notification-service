using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories;

public class TokenRepository : ITokenRepository
{
    public Task<Either<Failure, string>> Generate(UserEntity user)
    {
        throw new NotImplementedException();
    }
}