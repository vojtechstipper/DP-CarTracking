using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class AddedVehicleLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VehicleLocations",
                schema: "CarTrackingData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Radius = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleLocations_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "CarTrackingData",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleLocations_VehicleId",
                schema: "CarTrackingData",
                table: "VehicleLocations",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleLocations",
                schema: "CarTrackingData");
        }
    }
}
