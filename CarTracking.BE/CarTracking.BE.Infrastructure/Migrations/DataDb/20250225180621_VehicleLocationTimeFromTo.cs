using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class VehicleLocationTimeFromTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "FromTime",
                schema: "CarTrackingData",
                table: "VehicleLocations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToTime",
                schema: "CarTrackingData",
                table: "VehicleLocations",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromTime",
                schema: "CarTrackingData",
                table: "VehicleLocations");

            migrationBuilder.DropColumn(
                name: "ToTime",
                schema: "CarTrackingData",
                table: "VehicleLocations");
        }
    }
}
