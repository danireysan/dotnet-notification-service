using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});
    


builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(connectionString,
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "users")));


builder.Services.AddScoped<IPasswordHasher<UserId>, PasswordHasher<UserId>>();
builder.Services.AddScoped<ICustomPasswordHasher, PasswordHashingService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();

var app = builder.Build();
if (args.Contains("--migrate"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<UserContext>();

    for (var i = 0; i < 5; i++)
    {
        try
        {
            logger.LogInformation("Attempting to apply migrations (Attempt {Attempt})...", i + 1);
            await context.Database.MigrateAsync();
            logger.LogInformation("Migrations applied successfully.");
            break; // Success!
        }
        catch (Exception ex)
        {
            if (i == 4) // Last attempt failed
            {
                logger.LogCritical(ex, "Could not apply migrations after multiple attempts.");
                throw;
            }

            logger.LogWarning("Postgres not ready yet. Retrying in 2s...");
            await Task.Delay(2000);
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options => { options.DocumentPath = "/openapi/v1.json"; });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();