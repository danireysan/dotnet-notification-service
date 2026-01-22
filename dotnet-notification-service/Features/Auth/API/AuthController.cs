using dotnet_notification_service.Features.Auth.Application;
using dotnet_notification_service.Features.Auth.Domain.Entities;


using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Auth.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ICreateUserUsecase _createUserResultUsecase;
        public AuthController(ICreateUserUsecase createUserResultUsecase)
        {
            _createUserResultUsecase = createUserResultUsecase;
        }


        [HttpPost]
        public async Task<ActionResult<UserAccessedEntity>> CreateUser([FromBody] CreateUserRequestEntity request)
        {

            var result = await _createUserResultUsecase.CallAsync(request);

            return result.Match<ActionResult<UserAccessedEntity>>(
                failure => BadRequest(new { Message = failure.Message }),
                createUserResult => Created(string.Empty, createUserResult)
            );
            
        }
    }
}
