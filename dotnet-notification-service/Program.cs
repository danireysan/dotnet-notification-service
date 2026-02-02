using dotnet_notification_service.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// TODO: add context
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
//     builder.Configuration.GetConnectionString("DefaultConnection")
// ));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
