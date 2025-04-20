using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class AddedUserIdsToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "CarTrackingData",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Password",
                schema: "CarTrackingData",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "Username",
                schema: "CarTrackingData",
                table: "Accounts",
                newName: "UserIds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserIds",
                schema: "CarTrackingData",
                table: "Accounts",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "CarTrackingData",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "CarTrackingData",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
