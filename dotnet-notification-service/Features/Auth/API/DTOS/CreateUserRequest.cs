namespace dotnet_notification_service.Features.Auth.API.DTOS;

public record CreateUserRequest(
    string Email,
    string Password
);