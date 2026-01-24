namespace dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;

public sealed record UserId
{
    public string Value { get; }

    public UserId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            // TODO: dont throw use failure
            throw new ArgumentException("Password hash cannot be empty");

        Value = value;
    }

    public override string ToString() => Value;
}