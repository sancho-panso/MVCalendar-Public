using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class eventUpdateAllReceiverProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendToAll",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendToAll",
                table: "Events");
        }
    }
}
