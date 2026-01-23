namespace dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;

public record CreateUserResult(
    string Token,
    string UserId
);
