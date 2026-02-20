using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_notification_service.Migrations.Notification
{
    /// <inheritdoc />
    public partial class AddSendMetaDataToNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SendMetadata",
                schema: "notifications",
                table: "Notifications",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendMetadata",
                schema: "notifications",
                table: "Notifications");
        }
    }
}
