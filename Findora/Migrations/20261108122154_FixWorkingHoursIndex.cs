using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Findora.Migrations
{
    /// <inheritdoc />
    public partial class FixWorkingHoursIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkingHours_PlaceId_Day_OpenTime",
                table: "WorkingHours");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_PlaceId_Day_OpenTime",
                table: "WorkingHours",
                columns: new[] { "PlaceId", "Day", "OpenTime" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkingHours_PlaceId_Day_OpenTime",
                table: "WorkingHours");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingHours_PlaceId_Day_OpenTime",
                table: "WorkingHours",
                columns: new[] { "PlaceId", "Day", "OpenTime" },
                unique: true,
                filter: "[OpenTime] IS NOT NULL");
        }
    }
}
