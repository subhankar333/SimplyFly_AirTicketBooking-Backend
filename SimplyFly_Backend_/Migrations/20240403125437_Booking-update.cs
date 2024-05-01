using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyFly_Project.Migrations
{
    public partial class Bookingupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Bookings",
                newName: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Bookings",
                newName: "UserId");
        }
    }
}
