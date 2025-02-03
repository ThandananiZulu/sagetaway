using Microsoft.EntityFrameworkCore.Migrations;

namespace sagetaway.Migrations
{
    public partial class DeleteHotelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ensure the table exists before dropping it (if necessary)
            migrationBuilder.DropTable(
                name: "Hotel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotel",
                columns: table => new
                {
                    // Define the columns exactly as they were before deletion.
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: true)
                    // Add any other columns that originally existed.
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotel", x => x.Id);
                });
        }
    }
}
