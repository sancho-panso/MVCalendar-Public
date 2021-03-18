using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class dbsetForFamily : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Family_FamilyID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Family",
                table: "Family");

            migrationBuilder.RenameTable(
                name: "Family",
                newName: "Families");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Families",
                table: "Families",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Families_FamilyID",
                table: "AspNetUsers",
                column: "FamilyID",
                principalTable: "Families",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Families_FamilyID",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Families",
                table: "Families");

            migrationBuilder.RenameTable(
                name: "Families",
                newName: "Family");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Family",
                table: "Family",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Family_FamilyID",
                table: "AspNetUsers",
                column: "FamilyID",
                principalTable: "Family",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
