using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarTracking.BE.Infrastructure.Migrations.DataDb
{
    /// <inheritdoc />
    public partial class AddedTripId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripId",
                schema: "CarTrackingData",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripId",
                schema: "CarTrackingData",
                table: "Statuses");
        }
    }
}
