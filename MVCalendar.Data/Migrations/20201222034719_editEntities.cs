using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class editEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Surnaame",
                table: "AspNetUsers",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "Birhday",
                table: "AspNetUsers",
                newName: "Birthday");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "FamilyID",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Family",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Family", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FamilyID",
                table: "AspNetUsers",
                column: "FamilyID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Family_FamilyID",
                table: "AspNetUsers",
                column: "FamilyID",
                principalTable: "Family",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Family_FamilyID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Family");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FamilyID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FamilyID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "AspNetUsers",
                newName: "Surnaame");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "AspNetUsers",
                newName: "Birhday");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
