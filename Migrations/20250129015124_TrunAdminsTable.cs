using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sagetaway.Migrations
{
    /// <inheritdoc />
    public partial class TrunAdminsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE Admins;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
