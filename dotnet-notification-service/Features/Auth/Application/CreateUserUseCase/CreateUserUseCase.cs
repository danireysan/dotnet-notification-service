using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using Funcky.Monads;
using UnprocessableEntity = Microsoft.AspNetCore.Http.HttpResults.UnprocessableEntity;

namespace dotnet_notification_service.Features.Auth.Application.CreateUserUseCase
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        public CreateUserUseCase(IUserRepository userRepository, ICustomPassWordHasher hasher, ITokenRepository tokenRepository)
        {
            userRepository = new UserRepository();
            hasher = new CustomPasswordHasher();
            tokenRepository = new TokenRepository();

        }

        public async Task<Either<Failure, CreateUserResult>> CallAsync(CreateUserCommand @params)
        {
            return 
                from email in EmailAddress.Create(@params.Email) 
                select new CreateUserResult("", "");
        }
    }
}