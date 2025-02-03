using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sagetaway.Migrations
{
    /// <inheritdoc />
    public partial class createAttractionsAndTraspoAndAlterHotelTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NameOfPlace",
                table: "AdminHotels",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "HotelInformation",
                table: "AdminHotels",
                newName: "Information");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AdminHotels",
                newName: "NameOfPlace");

            migrationBuilder.RenameColumn(
                name: "Information",
                table: "AdminHotels",
                newName: "HotelInformation");
        }
    }
}
