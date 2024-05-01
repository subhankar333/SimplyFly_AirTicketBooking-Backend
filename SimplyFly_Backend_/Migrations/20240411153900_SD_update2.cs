using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyFly_Project.Migrations
{
    public partial class SD_update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlightNumber",
                table: "SeatDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatDetails_FlightNumber",
                table: "SeatDetails",
                column: "FlightNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatDetails_Flights_FlightNumber",
                table: "SeatDetails",
                column: "FlightNumber",
                principalTable: "Flights",
                principalColumn: "FlightNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatDetails_Flights_FlightNumber",
                table: "SeatDetails");

            migrationBuilder.DropIndex(
                name: "IX_SeatDetails_FlightNumber",
                table: "SeatDetails");

            migrationBuilder.DropColumn(
                name: "FlightNumber",
                table: "SeatDetails");
        }
    }
}
