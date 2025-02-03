using Microsoft.EntityFrameworkCore.Migrations;

namespace sagetaway.Migrations
{
    public partial class CreateAdminHotelsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminHotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image1 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Image2 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Image3 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Image4 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Image5 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    NameOfPlace = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GoogleMapsEmbed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotelInformation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminHotels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminHotels");
        }
    }
}
