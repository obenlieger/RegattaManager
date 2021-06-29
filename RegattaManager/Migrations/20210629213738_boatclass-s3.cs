using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class boatclasss3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Boatclasses",
                columns: new[] { "BoatclassId", "Name", "Seats" },
                values: new object[] { 12, "S3", 3 });

            migrationBuilder.InsertData(
                table: "RaceTyps",
                columns: new[] { "RaceTypId", "Name", "isFinal" },
                values: new object[,]
                {
                    { 1, "Vorlauf", false },
                    { 2, "Zwischenlauf", false },
                    { 3, "Hoffnungslauf", false },
                    { 4, "Endlauf", true }
                });

            migrationBuilder.InsertData(
                table: "Racestati",
                columns: new[] { "RacestatusId", "Name" },
                values: new object[,]
                {
                    { 1, "geplant" },
                    { 2, "gestartet" },
                    { 3, "abgenommen" },
                    { 4, "abgebrochen" },
                    { 1002, "beendet" },
                    { 1003, "wird ausgelost" },
                    { 1004, "ist ausgelost" },
                    { 1005, "Auslosung bestätigt" },
                    { 1006, "zu wenig Teilnehmer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Boatclasses",
                keyColumn: "BoatclassId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RaceTyps",
                keyColumn: "RaceTypId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RaceTyps",
                keyColumn: "RaceTypId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RaceTyps",
                keyColumn: "RaceTypId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RaceTyps",
                keyColumn: "RaceTypId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "Racestati",
                keyColumn: "RacestatusId",
                keyValue: 1006);
        }
    }
}
