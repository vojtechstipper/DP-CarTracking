using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class StatusNamedLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUsedForVirtualGarage",
                schema: "CarTrackingData",
                table: "VehicleLocations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NamedVehicleLocationId",
                schema: "CarTrackingData",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUsedForVirtualGarage",
                schema: "CarTrackingData",
                table: "VehicleLocations");

            migrationBuilder.DropColumn(
                name: "NamedVehicleLocationId",
                schema: "CarTrackingData",
                table: "Statuses");
        }
    }
}
