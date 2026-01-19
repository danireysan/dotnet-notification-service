namespace dotnet_notification_service.Features.Auth.Domain.Entities
{
    public class CreateUserResultEntity
    {
        public required string UserId { get; init; }
        public required string Token { get; init; }
    }
}