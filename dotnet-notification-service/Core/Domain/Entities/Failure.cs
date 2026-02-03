namespace dotnet_notification_service.Core.Domain.Entities;

// Base record
public abstract record Failure
{
    public required string Message { get; init; }
}

// 401
public record UnauthorizedFailure : Failure;

// 422
public record UnprocessableEntityFailure : Failure;
// 409
public record ConflictFailure : Failure;
// 500
public record ServerFailure : Failure;
