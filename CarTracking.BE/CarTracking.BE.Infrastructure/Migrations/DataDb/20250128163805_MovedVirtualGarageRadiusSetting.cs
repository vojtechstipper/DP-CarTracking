using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class MovedVirtualGarageRadiusSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VirtualGarage_Radius",
                schema: "CarTrackingData",
                table: "Vehicles",
                newName: "Settings_Radius");

            migrationBuilder.AlterColumn<double>(
                name: "Settings_Radius",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Settings_Radius",
                schema: "CarTrackingData",
                table: "Vehicles",
                newName: "VirtualGarage_Radius");

            migrationBuilder.AlterColumn<double>(
                name: "VirtualGarage_Radius",
                schema: "CarTrackingData",
                table: "Vehicles",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
