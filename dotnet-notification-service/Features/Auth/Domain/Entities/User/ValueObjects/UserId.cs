using NUlid;

namespace dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;

public sealed record UserId(Ulid Value)
{
    public static UserId New() => new(Ulid.NewUlid());

    public override string ToString() => Value.ToString();
}