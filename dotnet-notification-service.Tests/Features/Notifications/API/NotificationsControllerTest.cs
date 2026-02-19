using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.API.Controllers;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;
using dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Infra.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUlid;
using System.Security.Claims;
using dotnet_notification_service.Tests.TestHelpers;
using Testcontainers.PostgreSql;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationsControllerTest : IAsyncLifetime


{

    private NotificationEntity _notification;
    private readonly ILogger<NotificationsController> _logger = Mock.Of<ILogger<NotificationsController>>();
    private readonly ILogger<EmailSender> _emailLogger = Mock.Of<ILogger<EmailSender>>();
    private readonly IOptions<SmtpOptions > _smtpOptions = Mock.Of<IOptions<SmtpOptions>>();
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16")
        .Build();

    private readonly ControllerContext _authContextStub = ControllerContextStub.Create(true, "user-id", "daniel@gmail.com");

    private readonly ControllerContext _noAuthContextStub =
        ControllerContextStub.Create(false, "user-id", "daniel@gmail.com");

    private NotificationContext? _context;
    private NotificationRepository? _repository;
    private ICreateNotificationUseCase _createUseCase;
    private IUpdateNotificationUseCase _updateNotification;
    private IGetNotificationsUseCase _getNotificationsUseCase;
    private IDeleteNotificationsUseCase _deleteNotificationUseCase;
    private NotificationsController _sut;
    public async Task InitializeAsync()
    {
        var ulid = Ulid.NewUlid();
        _notification = new NotificationEntity(ulid,"Title", "Content", "Recipient", "CreatedBy", NotificationChannel.Push);
        
        
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<NotificationContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;

        _context = new NotificationContext(options);

        await _context.Database.EnsureCreatedAsync();
        _repository = new NotificationRepository(_context);
        var emailSender = new EmailSender(_emailLogger, _smtpOptions);
        _createUseCase = new CreateNotificationUseCase([emailSender], _repository);
        _updateNotification = new UpdateNotificationUseCase(_repository);
        _deleteNotificationUseCase = new DeleteNotificationsUseCase(_repository);
        _getNotificationsUseCase = new GetNotificationsUseCase(_repository);

        _sut = new NotificationsController(_createUseCase, _updateNotification, _deleteNotificationUseCase, _getNotificationsUseCase, _logger);
    }
    // --- 1. POST /CreateNotification ---
    [Fact]
    public async Task CreateNotification_ShouldReturnUnauthorized_WhenThereIsNoToken()
    {
        //? Arrange
        _sut.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                // Provide an empty ClaimsPrincipal so controller.User is not null (avoids ArgumentNullException)
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };


        var dto = new EmailNotificationDto(null,"Hi", "Body", "test@example.com");

        //? Act
        var createResult = await _sut.CreateNotification(dto);


        //? Assert
        Assert.IsType<UnauthorizedObjectResult>(createResult);
    }

    [Fact]
    public async Task CreateNotification_HappyPath_ReturnsOkAndPersistsData()
    {
        // Setup: Authenticate + Valid DTO
        // Reset DB
        _context!.RemoveRange(_context.Notifications);
        await _context.SaveChangesAsync();

        // Auth
        _sut.ControllerContext = _authContextStub;
        var notification = new EmailNotificationDto(null, "Title", "Content", "Recipient");
        // Act: POST /api/notifications
        var result = await _sut.CreateNotification(notification);
        // Assert: 200 OK + Verify DB record via Testcontainer
        Assert.IsType<OkObjectResult>(result);

        // Assert - Database
        var notificationsInDb = await _context.Notifications.ToListAsync();

        Assert.Single(notificationsInDb);

        var saved = notificationsInDb.First();
        Assert.Equal("Title", saved.Title);
        Assert.Equal("Content", saved.Content);
        Assert.Equal("Recipient", saved.Recipient);
    }

    [Fact]
    public async Task CreateNotification_InvalidDto_Returns400BadRequest()
    {
        // Setup: Authenticate + DTO with empty fields
        // Act: POST /api/notifications
        // Assert: 400 Bad Request
    }

    [Fact]
    public async Task CreateNotification_DomainLogicFailure_ReturnsMappedError()
    {
        // Setup: Authenticate + DTO that triggers a Failure (e.g., duplicate)
        // Act: POST /api/notifications
        // Assert: e.g., 409 Conflict (via FailureMapperExtension)
    }

    // --- 2. GET /GetNotifications ---
    [Fact]
    public async Task GetNotifications_UserIsolation_OnlyReturnsOwnData()
    {
        // Setup: Seed DB with data for User A and User B. Authenticate as User A.
        // Act: GET /api/notifications
        // Assert: List count matches User A only; User B's data is absent.
    }

    [Fact]
    public async Task GetNotifications_EmptyState_ReturnsOkWithEmptyList()
    {
        // Setup: Authenticate new user with zero records in DB
        // Act: GET /api/notifications
        // Assert: 200 OK + Json body is []
    }

    // --- 3. PUT & DELETE (Future-Proofing) ---

    [Fact]
    public async Task UpdateNotification_DifferentUserOwnership_Returns403Or404()
    {
        // Setup: User A tries to update User B's notification ID
        // Act: PUT /api/notifications
        // Assert: 403 Forbidden or 404 Not Found
    }

    [Fact]
    public async Task DeleteNotification_Idempotency_Returns404OnSecondCall()
    {
        // Setup: Authenticate + Existing ID
        // Act: DELETE once, then DELETE again
        // Assert: First call 200/204; Second call 404 Not Found
    } 
    
    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}