using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace dotnet_notification_service.Tests.TestHelpers;

/// <summary>
/// Helper to build ControllerContext and ClaimsPrincipal stubs for unit tests.
/// Use ControllerContextStub.Create(authenticated: true) to get an authenticated context.
/// </summary>
public static class ControllerContextStub
{
    public static ControllerContext Create(bool authenticated = false, string? userId = null, string? email = null)
    {
        var ctx = new DefaultHttpContext();
        if (authenticated)
        {
            userId ??= "test-user-id";
            email ??= "test@example.com";
            ctx.User = CreateUserPrincipal(userId, email);
        }
        else
        {
            ctx.User = new ClaimsPrincipal(new ClaimsIdentity());
        }

        return new ControllerContext { HttpContext = ctx };
    }

    private static ClaimsPrincipal CreateUserPrincipal(string userId, string email)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, email)
        };

        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuth"));
    }
}
