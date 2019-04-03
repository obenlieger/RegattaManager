using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class ReportedRaceComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "ReportedRaces",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "ReportedRaces");
        }
    }
}
