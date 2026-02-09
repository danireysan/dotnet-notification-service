using dotnet_notification_service.Core.Domain.Entities;
using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using dotnet_notification_service.Features.Notifications.Domain.Repository;
using Funcky;
using Funcky.Monads;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

using EitherUnit = Either<Failure, Unit>;
public class SmsSender : INotificationSender
{
    private readonly TwilioOptions _options;
    private readonly ILogger<SmsSender> _logger;
    public SmsSender(IOptions<TwilioOptions> config, ILogger<SmsSender> logger)
    {
        _options = config.Value;
        _logger = logger;
        TwilioClient.Init(_options.AccountSid, _options.AuthToken);
    } 

    public NotificationChannel Channel => NotificationChannel.Sms;

    public async Task<EitherUnit> Send(NotificationEntity dto)
    {

        var recipient = dto.Recipient?.Trim();
        if (string.IsNullOrEmpty(recipient))
        {

            var failure = new UnprocessableEntityFailure { Message = "Recipient phone number is required." };
            return EitherUnit.Left(failure);
        }
        
        if (!IsValidE164(recipient))
        {
            var failure = new UnprocessableEntityFailure { Message = $"Recipient phone number is not a valid E.164 phone number: '{recipient}'" };
            return EitherUnit.Left(failure);
        }
        
        try
        {
            await MessageResource.CreateAsync(
                to: new PhoneNumber(recipient),
                from: new PhoneNumber(_options.FromPhoneNumber),
                body: dto.Content
            );
            return new Unit();
        }
        catch (Exception e)
        {
            var detailedMessage = e.InnerException?.Message ?? e.Message;
            var failure = new ServerFailure
            {
                Message = $"SMS send failed because: {detailedMessage}"
            };

            return EitherUnit.Left(failure);
        }
    }
    
    private static readonly Regex E164Regex = new(@"^\+[1-9]\d{1,14}$", RegexOptions.Compiled);

    private static bool IsValidE164(string number) => E164Regex.IsMatch(number);
}