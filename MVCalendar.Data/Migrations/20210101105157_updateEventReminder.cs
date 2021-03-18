using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCalendar.Data.Migrations
{
    public partial class updateEventReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendReminder",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendReminder",
                table: "Events");
        }
    }
}
