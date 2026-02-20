using System.Text;
using Asp.Versioning;
using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Auth.Application.CreateUserUseCase;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using dotnet_notification_service.Features.Auth.Domain.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories;
using dotnet_notification_service.Features.Auth.Infra.Repositories.User;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;
using dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using dotnet_notification_service.Features.Notifications.Domain.Services;
using dotnet_notification_service.Features.Notifications.Infra.Repository;
using dotnet_notification_service.Features.Notifications.Infra.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(
    options => options.IncludeScopes = true);
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter your JWT token **without** the 'Bearer ' prefix. Just paste the token value.",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT"
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes.Add("Bearer", securityScheme);
        
        var securitySchemeReference = new OpenApiSecuritySchemeReference("Bearer");
        document.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                [securitySchemeReference] = []
            }
        };
        document.Info.Title = "Notification Service API";
        document.Info.Version = "v1";
        document.Info.Description = "API for managing notifications via Email, SMS, and Push channels.";
        return Task.CompletedTask;
    });
});

// Config api versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Parse JWT config
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .ValidateDataAnnotations() 
    .ValidateOnStart();

// Parse SMTP config
builder.Services.AddOptions<SmtpOptions>()
    .Bind(builder.Configuration.GetSection(SmtpOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Parse Twilio config
builder.Services.AddOptions<TwilioOptions>()
    .Bind(builder.Configuration.GetSection(TwilioOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();


    try
    {
        FirebaseApp.Create(new AppOptions()
        {
            Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dotnet-notification-service-firebase-adminsdk-fbsvc-871b0faa7c.json")),
        });
        Console.WriteLine($"Firebase initialized using credentials file: ");
    }
    catch (Exception ex)
    {
        // Don't allow Firebase initialization failures to crash the application startup.
        Console.WriteLine($"Warning: Failed to initialize Firebase: {ex.Message}");
    }


// Init EF contexts
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(connectionString,
        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "users")));

builder.Services.AddDbContext<NotificationContext>(options =>
    options.UseNpgsql(connectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", "notifications")));

// Inject Auth Dependencies
builder.Services.AddScoped<IPasswordHasher<UserId>, PasswordHasher<UserId>>();
builder.Services.AddScoped<ICustomPasswordHasher, PasswordHashingService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();

// Inject Notification Dependencies
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<INotificationSender, EmailSender>();
builder.Services.AddScoped<INotificationSender, SmsSender>();
builder.Services.AddScoped<INotificationSender, PushSender>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICreateNotificationUseCase, CreateNotificationUseCase>();
builder.Services.AddScoped<IUpdateNotificationUseCase, UpdateNotificationUseCase>();
builder.Services.AddScoped<IDeleteNotificationsUseCase, DeleteNotificationsUseCase>();
builder.Services.AddScoped<IGetNotificationsUseCase, GetNotificationsUseCase>();

// JWT config
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();


builder.Services.PostConfigure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
{

    var jwtSettings = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();
    
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings!.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"JWT Token validated for: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        },
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            Console.WriteLine($"JWT Token received: {(string.IsNullOrEmpty(token) ? "NO TOKEN" : token[..Math.Min(50, token.Length)] + "...")}");
            return Task.CompletedTask;
        }
    };
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Exposes the JSON/YAML doc
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "/openapi/v1.json";
        options.DocumentTitle = "Notification Service API - Dev Mode";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

