using dotnet_notification_service.Features.Notifications.Domain.Services;

namespace dotnet_notification_service.Features.Notifications.Infra.Services;

public class EmailTemplateService : IEmailTemplateService
{
    public string GenerateEmailTemplate(string title, string content)
    {
        var templatePath = "dotnet-notification-service/Features/Notifications/Templates/email_template.html";
        var template = File.ReadAllText(templatePath);
        var year = DateTime.Now.Year;
        
        return template
                .Replace("{{TITLE}}", title)
                .Replace("{{CONTENT}}", content)
                .Replace("{{YEAR}}", year.ToString());
        
    }
}