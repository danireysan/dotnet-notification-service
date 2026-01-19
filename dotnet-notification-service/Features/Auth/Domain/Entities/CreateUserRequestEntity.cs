namespace dotnet_notification_service.Features.Auth.Domain.Entities
{
    public class CreateUserRequestEntity
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}