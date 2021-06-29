using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class startboatstati : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Startboatstati",
                columns: new[] { "StartboatstatusId", "Name" },
                values: new object[,]
                {
                    { 1, "Am Start" },
                    { 2, "Im Rennen" },
                    { 3, "Im Ziel" },
                    { 4, "gekentert" },
                    { 5, "Nicht am Start" },
                    { 6, "Gemeldet" },
                    { 7, "falsch eingefahren" },
                    { 8, "disqualifiziert" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Startboatstati",
                keyColumn: "StartboatstatusId",
                keyValue: 8);
        }
    }
}
