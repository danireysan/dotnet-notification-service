using System.Text.Json;
using dotnet_notification_service.Features.Notifications.API.DTOs;
using Xunit.Abstractions;

namespace dotnet_notification_service.Tests.Features.Notifications.API;

public class NotificationTestData : IXunitSerializable
{
    public CreateNotificationDto Dto { get; set; } = null!;

    // Required for xUnit
    public NotificationTestData() { } 

    public NotificationTestData(CreateNotificationDto dto) => Dto = dto;

    public void Serialize(IXunitSerializationInfo info)
    {
        // Use System.Text.Json (which already has your polymorphic attributes)
        var json = JsonSerializer.Serialize(Dto);
        info.AddValue("json", json);
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
        var json = info.GetValue<string>("json");
        Dto = JsonSerializer.Deserialize<CreateNotificationDto>(json) ?? throw new InvalidOperationException();
    }

    // Override ToString so the Test Explorer shows a nice name
    public override string ToString() => $"{Dto.Channel}: {Dto.Title}";
}