using System.ComponentModel.DataAnnotations;

namespace dotnet_notification_service.Core.Domain.Entities;

public class SmtpOptions
{
    public const string SectionName = "Smtp";

    [Required(AllowEmptyStrings = false)]
    public string Mail { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = string.Empty;
}