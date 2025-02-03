using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sagetaway.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminHotelsTableTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.CreateTable(
                name: "AdminHotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image1 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Image2 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Image3 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Image4 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    Image5 = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
                    NameOfPlace = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    ContactNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GoogleMapsEmbed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelInformation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminHotels", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminHotels");

            migrationBuilder.CreateTable(
                name: "Hotel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotel", x => x.Id);
                });
        }
    }
}
