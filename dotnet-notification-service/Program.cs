using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Infra.Repositories.User;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(connectionString,
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "users")));


var app = builder.Build();

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