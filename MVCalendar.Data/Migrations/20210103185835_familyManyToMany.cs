using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class familyManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Families_FamilyID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FamilyID",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "FamiliesCalendarUsers",
                columns: table => new
                {
                    CalendarUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FamilyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamiliesCalendarUsers", x => new { x.CalendarUserId, x.FamilyId });
                    table.ForeignKey(
                        name: "FK_FamiliesCalendarUsers_AspNetUsers_CalendarUserId",
                        column: x => x.CalendarUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FamiliesCalendarUsers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FamiliesCalendarUsers_FamilyId",
                table: "FamiliesCalendarUsers",
                column: "FamilyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FamiliesCalendarUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "FamilyID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyID",
                table: "AspNetUsers",
                column: "FamilyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Families_FamilyID",
                table: "AspNetUsers",
                column: "FamilyID",
                principalTable: "Families",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
