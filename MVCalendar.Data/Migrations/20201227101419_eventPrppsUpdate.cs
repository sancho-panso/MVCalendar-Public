using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class eventPrppsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Events",
                newName: "EventStatus");

            migrationBuilder.AddColumn<Guid>(
                name: "FamilyID",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                name: "IX_Events_FamilyID",
                table: "Events",
                column: "FamilyID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Families_FamilyID",
                table: "Events",
                column: "FamilyID",
                principalTable: "Families",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_MesageToId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_MessageFromId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Families_FamilyID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_FamilyID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MesageToId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MessageFromId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FamilyID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MesageToId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MessageFromId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "EventStatus",
                table: "Events",
                newName: "Status");
        }
    }
}
