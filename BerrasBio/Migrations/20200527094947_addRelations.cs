using Microsoft.EntityFrameworkCore.Migrations;

namespace BerrasBio.Migrations
{
    public partial class addRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ViewingId",
                table: "Tickets",
                column: "ViewingId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SalonId",
                table: "Seats",
                column: "SalonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Salons_SalonId",
                table: "Seats",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "SalonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Viewings_ViewingId",
                table: "Tickets",
                column: "ViewingId",
                principalTable: "Viewings",
                principalColumn: "ViewingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Salons_SalonId",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Viewings_ViewingId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ViewingId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Seats_SalonId",
                table: "Seats");
        }
    }
}
