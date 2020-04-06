using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class uniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_WeekId",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_WeekId_IsMorning",
                table: "Prices",
                columns: new[] { "WeekId", "IsMorning" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prices_WeekId_IsMorning",
                table: "Prices");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_WeekId",
                table: "Prices",
                column: "WeekId");
        }
    }
}
