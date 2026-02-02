using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;

namespace dotnet_notification_service.Features.Auth.Domain.Entities.User;

public class UserEntity
{
    public UserId Id { get; }
    public EmailAddress Email { get; }
    public PasswordHash PasswordHash { get; }

    private UserEntity(UserId id, EmailAddress email, PasswordHash passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
    }


    public static UserEntity Create(
        UserId id,
        EmailAddress email,
        PasswordHash passwordHash
    )
    {
        return new UserEntity(id, email, passwordHash);
    }

    public static UserEntity Stub()
    {
        var defaultUserId = UserId.New();
        var defaultEmailAddress = EmailAddress.FromPersistence("danireysan@gmail.com");
        var passwordHash = new PasswordHash("asdasdfasdfasdff");
        return new UserEntity(defaultUserId, defaultEmailAddress, passwordHash);
    }
}