using Microsoft.EntityFrameworkCore.Migrations;

namespace BerrasBio.Migrations
{
    public partial class addedUsersAndMovieProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Order",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "userCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userCredentials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_userCredentials_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "userCredentials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_userCredentials_UserId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "userCredentials");

            migrationBuilder.DropIndex(
                name: "IX_Order_UserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Order");
        }
    }
}
