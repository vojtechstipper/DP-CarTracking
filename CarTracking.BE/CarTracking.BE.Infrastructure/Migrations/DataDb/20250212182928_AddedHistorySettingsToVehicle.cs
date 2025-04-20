using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class AddedHistorySettingsToVehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Settings_HistoryLenghtInDays",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 30);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settings_HistoryLenghtInDays",
                schema: "CarTrackingData",
                table: "Vehicles");
        }
    }
}
