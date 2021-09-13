using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UpdateTableAnnouncementAndTableClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Announcements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Weight",
                table: "Announcements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_ClientId",
                table: "Announcements",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_ClientId",
                table: "Announcements",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_ClientId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_ClientId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Announcements");
        }
    }
}
