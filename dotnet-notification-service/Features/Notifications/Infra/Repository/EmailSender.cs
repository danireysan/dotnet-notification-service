using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;


namespace dotnet_notification_service.Features.Notifications.Infra.Repository;


using EitherUnit = Either<Failure, Unit>;

public class EmailSender(ILogger<EmailSender> logger, IOptions<SmtpOptions> config) : INotificationSender
{
    public NotificationChannel Channel => NotificationChannel.Email;

    public async Task<EitherUnit> Send(NotificationEntity dto)
    {
        try
        {
            var message = new MimeMessage(); 
            message.From.Add(new MailboxAddress("Notification Service", config.Value.Mail));
            message.To.Add(new MailboxAddress("Recipient Name", dto.Recipient));
            message.Subject = dto.Title;
            message.Body = new TextPart("plain") { Text = dto.Content };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(config.Value.Mail, config.Value.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
            
            
            logger.LogInformation("Email sent");
            logger.LogInformation("Sent email notification");
            logger.LogInformation("Sending email notification");
            
            return  new Unit();
        }
        catch (Exception e)
        {
            
            var detailedMessage = e.InnerException?.Message ?? e.Message;
            logger.LogInformation("Failed email notification" + detailedMessage);
            var failure = new ServerFailure
            {
                Message = "Error sending email notification",
            };
            
            return EitherUnit.Left(failure);
        }
        
    }
}