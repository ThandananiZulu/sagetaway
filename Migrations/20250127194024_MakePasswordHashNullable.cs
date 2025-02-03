using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sagetaway.Migrations
{
    /// <inheritdoc />
    public partial class MakePasswordHashNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Alter the PasswordHash column to make it nullable
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: true,  // Set PasswordHash to be nullable
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the PasswordHash column back to non-nullable if needed
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Admins",
                type: "nvarchar(max)",
                nullable: false,  // Make PasswordHash non-nullable
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
