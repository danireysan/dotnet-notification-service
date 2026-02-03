using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.API.DTOS;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Auth.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiVersion(1)]
public class AuthController(ICreateUserUseCase createUserResultUseCase) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        var command = new CreateUserCommand(
            request.Email,
            request.Password
        );


        var result = await createUserResultUseCase.CallAsync(command);

        return result.Match<ActionResult<CreateUserResponse>>(
            failure => MapFailure(failure),
            success => CreatedAtAction(nameof(CreateUser), new CreateUserResponse(success.Token))
        );
    }

    ActionResult MapFailure(Failure failure) =>
        failure switch
        {
            UnauthorizedFailure => Unauthorized(new { error = failure.Message }),

            UnprocessableEntityFailure => UnprocessableEntity(new { error = failure.Message }),

            ConflictFailure => Conflict(new { error = failure.Message }),

            ServerFailure => StatusCode(500, new { error = failure.Message }),

            _ => StatusCode(500, new { error = "An unexpected error occurred: " + failure.Message })
        };
}