using System.ComponentModel.DataAnnotations;

namespace dotnet_notification_service.Core.Domain.Entities;

public class TwilioOptions
{
    public const string SectionName = "Twilio";
    
    [Required(AllowEmptyStrings = false)]
    public string AccountSid { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string AuthToken { get; set; } = string.Empty;
    [Required(AllowEmptyStrings = false)]
    public string FromPhoneNumber { get; set; } = string.Empty;
}