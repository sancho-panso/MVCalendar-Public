using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class eventUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_MesageToId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_MessageFromId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MesageToId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MessageFromId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MesageToId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MessageFromId",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "MesageTo",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageFrom",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MesageTo",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MessageFrom",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "MesageToId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageFromId",
                table: "Events",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_MesageToId",
                table: "Events",
                column: "MesageToId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_MessageFromId",
                table: "Events",
                column: "MessageFromId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_MesageToId",
                table: "Events",
                column: "MesageToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_MessageFromId",
                table: "Events",
                column: "MessageFromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
