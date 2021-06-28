using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class leistungsfeststellung : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RaceDraws",
                columns: new[] { "RaceDrawId", "Description", "EndlaufCount", "HoffnungslaufCount", "ReportedSBCountFrom", "ReportedSBCountTo", "VorlaufCount", "ZwischenlaufCount", "isAbteilungslauf" },
                values: new object[,]
                {
                    { 1, null, 1, 0, 1, 6, 0, 0, false },
                    { 2, "1.-3. in E", 1, 0, 7, 12, 2, 0, false },
                    { 3, null, 1, 0, 13, 18, 3, 1, false },
                    { 4, null, 1, 0, 19, 24, 4, 2, false },
                    { 5, null, 1, 1, 25, 30, 5, 2, false },
                    { 6, null, 1, 2, 31, 36, 6, 2, false },
                    { 7, null, 1, 2, 37, 42, 7, 3, false },
                    { 8, null, 1, 4, 43, 48, 8, 2, false },
                    { 16, null, 1, 3, 49, 54, 9, 3, false },
                    { 9, null, 1, 0, 1, 6, 0, 0, true },
                    { 10, null, 1, 0, 7, 12, 2, 0, true },
                    { 11, null, 1, 0, 13, 18, 3, 0, true },
                    { 12, null, 1, 0, 19, 24, 4, 0, true },
                    { 13, null, 1, 0, 25, 30, 5, 0, true },
                    { 14, null, 1, 0, 31, 36, 6, 0, true }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RaceDraws",
                keyColumn: "RaceDrawId",
                keyValue: 16);
        }
    }
}
