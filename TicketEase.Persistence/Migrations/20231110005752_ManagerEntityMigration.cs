using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketEase.Persistence.Migrations
{
    public partial class ManagerEntityMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Managers_ManagerAppUserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Managers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ManagerAppUserId",
                table: "AspNetUsers",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ManagerAppUserId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Managers_ManagerId",
                table: "AspNetUsers",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Managers_ManagerId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Managers",
                newName: "AppUserId");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "AspNetUsers",
                newName: "ManagerAppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ManagerId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ManagerAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Managers_ManagerAppUserId",
                table: "AspNetUsers",
                column: "ManagerAppUserId",
                principalTable: "Managers",
                principalColumn: "AppUserId");
        }
    }
}
