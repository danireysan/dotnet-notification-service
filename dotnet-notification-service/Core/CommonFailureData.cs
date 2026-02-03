using dotnet_notification_service.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace dotnet_notification_service.Core;

public class CommonFailureData : TheoryData<Type, int, Type>
{
    public CommonFailureData()
    {
        Add(typeof(UnauthorizedFailure), 401, typeof(UnauthorizedObjectResult));
        Add(typeof(UnprocessableEntityFailure), 422, typeof(UnprocessableEntityObjectResult));
        Add(typeof(ConflictFailure), 409, typeof(ConflictObjectResult));
        Add(typeof(ServerFailure), 500, typeof(ObjectResult));
    }
}