using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Core.Domain.Entities;

public static class FailureMapperExtension
{
    public static ActionResult MapFailure(ControllerBase controller, Failure failure)
    {
        return failure switch
        {
            UnauthorizedFailure => controller.Unauthorized(new ErrorResponse(failure.Message)),
            UnprocessableEntityFailure => controller.UnprocessableEntity(new ErrorResponse(failure.Message)),
            ForbiddenFailure => controller.Forbid(),
            NotFoundFailure => controller.NotFound(new ErrorResponse(failure.Message)),
            ConflictFailure => controller.Conflict(new ErrorResponse(failure.Message)),
            ServerFailure => controller.StatusCode(500, new ErrorResponse(failure.Message)),
            _ => controller.StatusCode(500, new ErrorResponse($"An unexpected error occurred: {failure.Message}"))
        };
    }
}