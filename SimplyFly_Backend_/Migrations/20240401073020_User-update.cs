using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimplyFly_Project.Migrations
{
    public partial class Userupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a new column for the varbinary(max) password
            migrationBuilder.AddColumn<byte[]>(
                name: "NewPassword",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            // Convert existing plaintext passwords to byte arrays and store them in the new column
            migrationBuilder.Sql("UPDATE Users SET NewPassword = CONVERT(varbinary(max), Password)");

            // Drop the old column
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            // Rename the new column to Password
            migrationBuilder.RenameColumn(
                name: "NewPassword",
                table: "Users",
                newName: "Password");

            // Add the Key column
            migrationBuilder.AddColumn<byte[]>(
                name: "Key",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the changes made in the Up method
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Key",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }


    }
}
