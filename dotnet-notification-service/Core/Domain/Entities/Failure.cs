namespace dotnet_notification_service.Core.Domain.Entities;

public class Failure
{
    public required string Message { get; init; }
}


// For 422 Error
public class UnprocessableEntityFailure : Failure
{
    
}