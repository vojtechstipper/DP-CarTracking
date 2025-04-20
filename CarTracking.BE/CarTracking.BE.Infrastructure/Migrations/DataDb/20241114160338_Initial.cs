using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CarTrackingData");

            migrationBuilder.CreateTable(
                name: "Accounts",
                schema: "CarTrackingData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "CarTrackingData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Settings_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAssignedToDevice = table.Column<bool>(type: "bit", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "CarTrackingData",
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "CarTrackingData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsAdminDevice = table.Column<bool>(type: "bit", nullable: false),
                    NotificationToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalSchema: "CarTrackingData",
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Devices_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "CarTrackingData",
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                schema: "CarTrackingData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Location_Latitude = table.Column<double>(type: "float", nullable: false),
                    Location_Longitude = table.Column<double>(type: "float", nullable: false),
                    Location_Speed = table.Column<double>(type: "float", nullable: true),
                    Location_Accuracy = table.Column<double>(type: "float", nullable: true),
                    Location_Altitude = table.Column<double>(type: "float", nullable: true),
                    Location_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BatteryInfo_ChargeLevel = table.Column<double>(type: "float", nullable: false),
                    BatteryInfo_IsEnergySaverOn = table.Column<bool>(type: "bit", nullable: false),
                    BatteryInfo_IsCharging = table.Column<bool>(type: "bit", nullable: false),
                    BatteryInfo_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Received = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statuses_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "CarTrackingData",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Statuses_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "CarTrackingData",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AccountId",
                schema: "CarTrackingData",
                table: "Devices",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_VehicleId",
                schema: "CarTrackingData",
                table: "Devices",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_DeviceId",
                schema: "CarTrackingData",
                table: "Statuses",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_VehicleId",
                schema: "CarTrackingData",
                table: "Statuses",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AccountId",
                schema: "CarTrackingData",
                table: "Vehicles",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statuses",
                schema: "CarTrackingData");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "CarTrackingData");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "CarTrackingData");

            migrationBuilder.DropTable(
                name: "Accounts",
                schema: "CarTrackingData");
        }
    }
}
