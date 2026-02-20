using Microsoft.AspNetCore.Mvc;

namespace dotnet_notification_service.Features.Health.API;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    // GET: api/health/ping
    [HttpGet("ping")]
    public IActionResult Ping()
    {
        return Ok("pong");
    }
}