using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class AddedVirtualGarage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "VirtualGarageIsEnabled",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VirtualGarage_Id",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VirtualGarage_Latitude",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VirtualGarage_Longitude",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VirtualGarage_Radius",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VirtualGarage_ValidTill",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VirtualGarageIsEnabled",
                schema: "CarTrackingData",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VirtualGarage_Id",
                schema: "CarTrackingData",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VirtualGarage_Latitude",
                schema: "CarTrackingData",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VirtualGarage_Longitude",
                schema: "CarTrackingData",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VirtualGarage_Radius",
                schema: "CarTrackingData",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VirtualGarage_ValidTill",
                schema: "CarTrackingData",
                table: "Vehicles");
        }
    }
}
