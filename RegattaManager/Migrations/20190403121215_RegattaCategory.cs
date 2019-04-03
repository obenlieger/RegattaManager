using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class RegattaCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Regattas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "Regattas",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartersLastYear",
                table: "Regattas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Mannschaftswertung",
                columns: table => new
                {
                    MWId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClubId = table.Column<int>(nullable: false),
                    ClubName = table.Column<string>(nullable: true),
                    OldclassName = table.Column<string>(nullable: true),
                    Wertung = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mannschaftswertung", x => x.MWId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mannschaftswertung");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "Regattas");

            migrationBuilder.DropColumn(
                name: "StartersLastYear",
                table: "Regattas");
        }
    }
}
