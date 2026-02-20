using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_notification_service.Migrations.Notification
{
    /// <inheritdoc />
    public partial class AddTimestampsToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                schema: "notifications",
                table: "Notifications",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentAt",
                schema: "notifications",
                table: "Notifications");
        }
    }
}
