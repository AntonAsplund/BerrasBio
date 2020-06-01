using Microsoft.EntityFrameworkCore.Migrations;

namespace BerrasBio.Migrations
{
    public partial class moviesProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_userCredentials_UserId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userCredentials",
                table: "userCredentials");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "userCredentials");

            migrationBuilder.RenameTable(
                name: "userCredentials",
                newName: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Movies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PerentalGuidance",
                table: "Movies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Plot",
                table: "Movies",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Movies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Users",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_UserId",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "PerentalGuidance",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Plot",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "userCredentials");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "userCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userCredentials",
                table: "userCredentials",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_userCredentials_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "userCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
