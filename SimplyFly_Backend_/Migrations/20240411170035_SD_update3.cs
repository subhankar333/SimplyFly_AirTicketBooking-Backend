using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyFly_Project.Migrations
{
    public partial class SD_update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "isBooked",
                table: "SeatDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isBooked",
                table: "SeatDetails");
        }
    }
}
