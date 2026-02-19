namespace dotnet_notification_service.Features.Notifications.Domain.Services;

public interface IEmailTemplateService
{
    string GenerateEmailTemplate(string title, string content);
}