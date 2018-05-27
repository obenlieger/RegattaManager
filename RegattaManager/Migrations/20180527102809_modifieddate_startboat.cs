using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RegattaManager.Migrations
{
    public partial class modifieddate_startboat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLate",
                table: "ReportedStartboats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "modifiedDate",
                table: "ReportedStartboats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLate",
                table: "ReportedStartboats");

            migrationBuilder.DropColumn(
                name: "modifiedDate",
                table: "ReportedStartboats");
        }
    }
}
