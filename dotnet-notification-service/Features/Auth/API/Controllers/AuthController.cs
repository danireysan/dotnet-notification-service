using Asp.Versioning;
using dotnet_notification_service.Features.Auth.API.DTOS;
using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Auth.API.Controllers
{
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
                failure => BadRequest(failure),
                createUserResult => Created(
                    nameof(CreateUser),
                    new CreateUserResponse(
                        createUserResult.Token
                    )
                ));
        }
    }
}