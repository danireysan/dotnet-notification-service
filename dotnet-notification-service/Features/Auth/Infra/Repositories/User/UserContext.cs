using dotnet_notification_service.Features.Auth.Domain.Entities.User;
using dotnet_notification_service.Features.Auth.Domain.Entities.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using NUlid;

namespace dotnet_notification_service.Features.Auth.Infra.Repositories.User;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("users");
        modelBuilder.Entity<UserEntity>(builder =>
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Id)
                .HasConversion(
                    id => id.Value.ToString(), 
                    value => new UserId(Ulid.Parse(value)) 
                )
                .IsRequired();
            
            builder.Property(u => u.Email)
                .HasConversion(
                    email => email.Value, 
                    value => EmailAddress.FromPersistence(value) 
                )
                .HasMaxLength(255)
                .IsRequired();
            
            builder.Property(u => u.PasswordHash)
                .HasConversion(
                    hash => hash.Value, 
                    value => new PasswordHash(value) 
                )
                .IsRequired();


            builder.HasIndex(u => u.Email).IsUnique();
        });
    }
}