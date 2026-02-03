using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.API.DTOS;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Auth.API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
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
            failure => FailureMapperExtension.MapFailure(this, failure),
            success => CreatedAtAction(nameof(CreateUser), new CreateUserResponse(success.Token))
        );
    }

    
}