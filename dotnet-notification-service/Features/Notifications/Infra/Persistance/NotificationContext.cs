using dotnet_notification_service.Features.Notifications.Domain.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace dotnet_notification_service.Features.Notifications.Infra.Repository;

public class NotificationContext(DbContextOptions<NotificationContext> options) : DbContext(options)
{
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("notifications");

        modelBuilder.Entity<NotificationEntity>(builder =>
            {
                builder.HasKey(e => e.NotificationId);
                builder.Property(e => e.NotificationId)
                    .HasConversion(
                        id => id.ToString(),
                        value => Ulid.Parse(value)
                    )
                    .IsRequired();
                
                builder.Property(e => e.Channel)
                    .HasConversion<string>();
                
                builder.Property(e => e.SendMetadata)
                    .HasColumnName("SendMetadata");
            }
        );
    }
}