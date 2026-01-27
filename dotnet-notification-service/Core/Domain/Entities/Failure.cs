namespace dotnet_notification_service.Core.Domain.Entities;

public class Failure
{
    public required string Message { get; init; }
}

// For 401
public class UnauthorizedFailure : Failure
{
    
}

// For 422 Error
public class UnprocessableEntityFailure : Failure
{
}

// for 409
public class ConflictFailure : Failure
{
    
}



// 500
public class ServerFailure : Failure
{
    
}
