namespace dotnet_notification_service.Features.Auth.Application;

public record CreateUserCommand(
    string Email,
    string Password
);