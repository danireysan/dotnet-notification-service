using System.Diagnostics;
using dotnet_notification_service.Features.Notifications.API.Controllers;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using dotnet_notification_service.Features.Notifications.Application.CreateNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.DeleteNotificationUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsFromUserUseCase;
using dotnet_notification_service.Features.Notifications.Application.GetNotificationsUseCase;
using dotnet_notification_service.Features.Notifications.Application.UpdateNotificationUsecase;
using dotnet_notification_service.Features.Notifications.Infra.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUlid;
using dotnet_notification_service.Tests.TestHelpers;
using Testcontainers.PostgreSql;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationsControllerTest : IAsyncLifetime


{
    
    private readonly ILogger<NotificationsController> _logger = Mock.Of<ILogger<NotificationsController>>();
   
    
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder("postgres:16")
        .Build();
    

    private readonly ControllerContext _authContextStub = ControllerContextStub.Create(true, "user-id", "daniel@gmail.com");

    private readonly ControllerContext _noAuthContextStub =
        ControllerContextStub.Create(false, "user-id", "daniel@gmail.com");

    private NotificationContext? _context;
    private NotificationRepository? _repository;
    private ICreateNotificationUseCase? _createUseCase;
    private IUpdateNotificationUseCase? _updateNotification;
    private IGetNotificationsUseCase? _getNotificationsUseCase;
    private IDeleteNotificationsUseCase? _deleteNotificationUseCase;
    private NotificationsController? _sut;
    public async Task InitializeAsync()
    {
        
        await _dbContainer.StartAsync();

        var options = new DbContextOptionsBuilder<NotificationContext>()
            .UseNpgsql(_dbContainer.GetConnectionString())
            .Options;

        _context = new NotificationContext(options);

        await _context.Database.EnsureCreatedAsync();
        _repository = new NotificationRepository(_context);
        var emailSender = new TestSender();
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
        _sut!.ControllerContext = _noAuthContextStub;
        var dto = new EmailNotificationDto(null,"Hi", "Body", "test@example.com");
        //? Act
        var createResult = await _sut.CreateNotification(dto);
        //? Assert
        Assert.IsType<UnauthorizedObjectResult>(createResult);
    }

    [Fact]
    public async Task CreateNotification_HappyPath_ReturnsOkAndPersistsData()
    {
        //? Arrange: Authenticate + Valid DTO
        await AuthSetup();
        var notification = new EmailNotificationDto(null, "Title", "Content", "Recipient");
        //? Act: POST /api/notifications
        var result = await _sut!.CreateNotification(notification);
        //? Assert: 200 OK + Verify DB record via Testcontainer
        Assert.IsType<OkObjectResult>(result);
        //? Assert - Database
        var notificationsInDb = await _context!.Notifications.ToListAsync();
        Assert.Single(notificationsInDb);
        var saved = notificationsInDb.First();
        Assert.Equal("Title", saved.Title);
        Assert.Equal("Content", saved.Content);
        Assert.Equal("Recipient", saved.Recipient);
    }
    
    // --- 2. GET /GetNotifications ---
    [Fact]
    public async Task GetNotifications_UserIsolation_OnlyReturnsOwnData()
    {
        //? Arrange: Seed DB with data for User A and User B. Authenticate as User A.
        // Login
        await AuthSetup();
        var userADto =  new EmailNotificationDto(null, "Title 1", "Content 1", "Recipient 1");
        await _sut!.CreateNotification(userADto);

        // Login and create another notification
        _sut.ControllerContext = ControllerContextStub.Create(true, "user-id-2", "daniel2@gmail.com");
        var userBDto = new EmailNotificationDto(null, "Title 2", "Content 2", "Recipient 2");
        await _sut.CreateNotification(userBDto);
        //? Act: GET /api/notifications
        var result = await _sut.GetNotifications();
        

        //? Assert: List count matches User B only; User A's data is absent.
        var notifications = ((OkObjectResult)result).Value as List<ResultNotificationDto>;
        Debug.Assert(notifications != null);
        // If we are getting an specific user notifications the easier test here is just to verify if they have one
        Assert.Single(notifications);
    }

    [Fact]
    public async Task GetNotifications_EmptyState_ReturnsOkWithEmptyList()
    {
        //? Arrange: Authenticate new user with zero records in DB
        await AuthSetup();
        //? Act: GET /api/notifications
        var result = await _sut!.GetNotifications();
        //? Assert: 200 OK + Json body is []
        Assert.IsType<OkObjectResult>(result);
        var notifications = ((OkObjectResult)result).Value as List<ResultNotificationDto>;
        Debug.Assert(notifications != null);
        Assert.Empty(notifications);
        
    }

    // --- 3. PUT & DELETE (Future-Proofing) ---
    
    

    [Fact]
    public async Task UpdateNotification_DifferentUserOwnership_Returns403()
    {
        //? Setup: User A tries to update User B's notification ID
        await AuthSetup();
        var userADto = new EmailNotificationDto(null, "Title 1", "Content 1", "Recipient 1");
        await _sut!.CreateNotification(userADto);
        var notification = await _context!.Notifications.FirstAsync();
        // Log with another user
        _sut.ControllerContext = ControllerContextStub.Create(true, "user-id-2", "daniel2@gmail.com");
        //? Act: PUT /api/notifications
        var userBDto = new EmailNotificationDto(notification.NotificationId.ToString(), "Title 2", "Content 2", "Recipient 2");
        var result = await _sut.UpdateNotification(userBDto);
        //? Assert: 403 Forbidden or 404 Not Found
        Assert.IsType<ForbidResult>(result);
    }
    
    [Fact]
    public async Task UpdateNotification_ValidDTO_ReturnsOk()
    {
        //? Setup: User A tries to update User B's notification ID
        await AuthSetup();
        var userADto = new EmailNotificationDto(null, "Title 1", "Content 1", "Recipient 1");
        await _sut!.CreateNotification(userADto);
        var notification = await _context!.Notifications.FirstAsync();
        var id = notification.NotificationId.ToString();
        _context.Entry(notification).State = EntityState.Detached;
        //? Act: PUT /api/notifications
        var updatedDto = new EmailNotificationDto(id, "Title 2", "Content 2", "Recipient 2");
        var result = await _sut.UpdateNotification(updatedDto);
        //? Assert: 403 Forbidden or 404 Not Found
        Assert.IsType<OkObjectResult>(result);
        var updatedNotification =  _context.Notifications.First();
        
        //? Assert - Database
        Assert.Equal(id, updatedNotification.NotificationId.ToString());
        Assert.Equal(updatedDto.Title, updatedNotification.Title);
        Assert.Equal(updatedDto.Content, updatedNotification.Content);
        Assert.Equal(updatedDto.Recipient, updatedNotification.Recipient);
    }
    
    [Fact]
    public async Task DeleteNotification_DifferentUserOwnership_Returns403()
    {
        //? Setup: User A tries to delete User B's notification ID
        await AuthSetup();
        var userADto = new EmailNotificationDto(null, "Title 1", "Content 1", "Recipient 1");
        await _sut!.CreateNotification(userADto);
        var notification = await _context!.Notifications.FirstAsync();
        // Log with another user
        _sut.ControllerContext = ControllerContextStub.Create(true, "user-id-2", "daniel2@gmail.com");
        //? Act: PUT /api/notifications
        var result = await _sut.DeleteNotification(notification.NotificationId.ToString());
        //? Assert: 403 Forbidden or 404 Not Found
        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task DeleteNotification_Idempotency_Returns404OnSecondCall()
    {
        //? Arrange: Authenticate + Existing ID
        await AuthSetup();
        var userDto = new EmailNotificationDto(null, "Title 1", "Content 1", "Recipient 1");
        await _sut!.CreateNotification(userDto);
        var notification = await _context!.Notifications.FirstAsync();
        //? Act: DELETE once, then DELETE again
        var firstDelete = await _sut.DeleteNotification(notification.NotificationId.ToString());
        var secondDelete = await _sut.DeleteNotification(notification.NotificationId.ToString());
        //? Assert: First call 200/204; Second call 404 Not Found
        Assert.IsType<NoContentResult>(firstDelete);
        Assert.IsType<NotFoundObjectResult>(secondDelete);
    }
    
    
    public async Task DisposeAsync()
    {
        await _context!.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }

    private async Task AuthSetup()
    {
        try
        {
            // Reset DB
            _context!.RemoveRange(_context.Notifications);
            await _context.SaveChangesAsync();

            // Auth
            _sut!.ControllerContext = _authContextStub;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}