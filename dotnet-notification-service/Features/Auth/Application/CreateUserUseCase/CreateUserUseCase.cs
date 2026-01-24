using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using Funcky.Monads;

namespace dotnet_notification_service.Features.Auth.Application.CreateUserUseCase
{
    public class CreateUserUseCase(
        IUserRepository userRepository,
        ICustomPasswordHasher hasher,
        ITokenRepository tokenRepository) : ICreateUserUseCase
    {
        public async Task<Either<Failure, CreateUserResult>> CallAsync(CreateUserCommand @params)
        {
            Failure? failure = null;
            try
            {
                var ensureMailIsUnique = await userRepository.EnsureMailIsUnique(@params.Email);
                var hashPassword = await hasher.HashPassword(@params.Email, @params.Password);
                var userEither =
                    from mail in EmailAddress.Create(@params.Email)
                    from _ in ensureMailIsUnique
                    from hash in hashPassword
                    select UserEntity.Create(
                        UserId.New(),
                        mail,
                        hash
                    );

                var user = userEither.GetOrElse(left =>
                {
                    failure = left;
                    throw new Exception(left.Message);
                });
                var addUser = await userRepository.Add(user);

                return from _ in addUser
                    select new CreateUserResult("", user.Id.ToString());
            }
            catch (Exception e)
            {
                return Either<Failure, CreateUserResult>.Left(failure ?? new ServerFailure
                {
                    Message = e.Message
                });
            }
        }
    }
}