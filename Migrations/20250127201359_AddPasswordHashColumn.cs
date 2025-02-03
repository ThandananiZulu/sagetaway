using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sagetaway.Migrations
{
    public partial class AddPasswordHashColumn : Migration
    {
        // This method will be used to apply the migration (e.g., adding the PasswordHash column)
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the PasswordHash column to the Admins table
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        // This method will be used to revert the migration (e.g., removing the PasswordHash column)
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the PasswordHash column from the Admins table
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Admins");
        }
    }
}
