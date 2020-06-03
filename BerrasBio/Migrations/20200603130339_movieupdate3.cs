using Microsoft.EntityFrameworkCore.Migrations;

namespace BerrasBio.Migrations
{
    public partial class movieupdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterURL",
                table: "Movies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosterURL",
                table: "Movies");
        }
    }
}
