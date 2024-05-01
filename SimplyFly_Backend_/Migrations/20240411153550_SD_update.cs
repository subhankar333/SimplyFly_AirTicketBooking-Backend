using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyFly_Project.Migrations
{
    public partial class SD_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeatDetails_Flights_FlightNumber1",
                table: "SeatDetails");

            migrationBuilder.DropIndex(
                name: "IX_SeatDetails_FlightNumber1",
                table: "SeatDetails");

            migrationBuilder.DropColumn(
                name: "FlightNumber1",
                table: "SeatDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FlightNumber1",
                table: "SeatDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatDetails_FlightNumber1",
                table: "SeatDetails",
                column: "FlightNumber1");

            migrationBuilder.AddForeignKey(
                name: "FK_SeatDetails_Flights_FlightNumber1",
                table: "SeatDetails",
                column: "FlightNumber1",
                principalTable: "Flights",
                principalColumn: "FlightNumber");
        }
    }
}
