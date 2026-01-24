using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using Funcky.Monads;
using UnprocessableEntity = Microsoft.AspNetCore.Http.HttpResults.UnprocessableEntity;

namespace dotnet_notification_service.Features.Auth.Application.CreateUserUseCase
{
    public class CreateUserUseCase(IUserRepository userRepository, ICustomPasswordHasher hasher, ITokenRepository tokenRepository) : ICreateUserUseCase
    {


        public async Task<Either<Failure, CreateUserResult>> CallAsync(CreateUserCommand @params)
        {

            var ensureMailIsUnique = await userRepository.EnsureMailIsUnique(@params.Email);
            var hashPassword = await hasher.HashPassword(@params.Email, @params.Password);
            return 
                from _ in EmailAddress.Create(@params.Email)
                from uniqueEmailAddress in ensureMailIsUnique
                from hash in hashPassword
                select new CreateUserResult("", "");

        }
    }
}