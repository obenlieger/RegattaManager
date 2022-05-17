using Microsoft.EntityFrameworkCore.Migrations;

namespace RegattaManager.Migrations
{
    public partial class datenbankeinträge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "MemberId", "Birthyear", "ClubId", "FirstName", "Gender", "LastName", "RentYear", "RentedToClubId", "isRented" },
                values: new object[,]
                {
                    { 2, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false },
                    { 3, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false },
                    { 4, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false },
                    { 5, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false },
                    { 6, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false },
                    { 7, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false },
                    { 8, 2000, 294, "WIRD", "M", "GESUCHT", 0, 0, false }
                });

            migrationBuilder.InsertData(
                table: "Oldclasses",
                columns: new[] { "OldclassId", "FromAge", "Name", "ToAge" },
                values: new object[] { 12, 32, "Senioren", 99 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "MemberId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Oldclasses",
                keyColumn: "OldclassId",
                keyValue: 12);
        }
    }
}
